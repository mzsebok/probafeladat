#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <stdlib.h>
#include <WinSock2.h>
#include <conio.h>
#include <time.h>


#define SRV_PORT 3939

#define MAXSPEED_D	250
#define MAXSPEED_R	30
#define WHEELSTEPVAL	10

HANDLE hSemRXData;
HANDLE hSemDisc;

typedef struct {
	char  ignition;
	char  gear;
	char  turnSignal;
	float speed;
	int   setWheelDegree;
	int   actWheelDegree;
	float acceleration;
}vehicleState;

vehicleState myVehicleState = { 0, 'N', 0, 0, 0, 0 , 0};		// actual vehicle state

typedef struct {
	char newData;
	char ignition;
	char gear;
	char turnSignal;
	int  speed;
	int  wheelDegree;
	int  acceleration;
	int  checkSum;
}rxData;

rxData newRxData = {0, 0, 'N', 0, 0, 0, 0, 0 };
//rxData newRxData = {0, 0, 'D', 0, 0, 320, 50, 0 };

char connectedState = 0;


int calclChecksum(rxData *recvData)
{
	int i;
	int chkSum = 0;
	char *data;

	data = recvData;
	i = sizeof(rxData) - sizeof(int);
	while ((i--) != 0)
	{
		chkSum += *(data++);
	}

	return (chkSum);
}

int parseReceivedData(rxData *recvData, char *smsg)
{
	rxData newRecvData;
	recvData->newData = 0;
	

	sscanf(smsg, "%d,%c,%d,%d,%d,%d,%d\n",
		&recvData->ignition,
		&recvData->gear,
		&recvData->turnSignal,
		&recvData->speed,
		&recvData->wheelDegree,
		&recvData->acceleration,
		&recvData->checkSum);

	if (calclChecksum(recvData) == recvData->checkSum)
		return (1);
	else
		return (0);
}

void vehicleStateMachine(void)
{
	int wheelStep = WHEELSTEPVAL;

	WaitForSingleObject(hSemRXData, INFINITE);

	if (newRxData.newData)
	{
		myVehicleState.ignition = newRxData.ignition;
		myVehicleState.gear = newRxData.gear;
		myVehicleState.turnSignal = newRxData.turnSignal;
		myVehicleState.acceleration = (float)newRxData.acceleration;
		myVehicleState.setWheelDegree = newRxData.wheelDegree;
		newRxData.newData = 0;
	}

	ReleaseSemaphore(hSemRXData, 1, 0);


	myVehicleState.speed += ((float)myVehicleState.acceleration * 5) / 100;
	if (myVehicleState.gear == 'D' && myVehicleState.speed > MAXSPEED_D)myVehicleState.speed = MAXSPEED_D;
	if (myVehicleState.gear == 'R' && myVehicleState.speed > MAXSPEED_R)myVehicleState.speed = MAXSPEED_R;
	if (myVehicleState.speed < 0)myVehicleState.speed = 0;


	if ((wheelStep = abs(myVehicleState.actWheelDegree - myVehicleState.setWheelDegree)) > WHEELSTEPVAL)wheelStep = WHEELSTEPVAL;

	if (myVehicleState.actWheelDegree > myVehicleState.setWheelDegree)myVehicleState.actWheelDegree -= wheelStep;
	else if(myVehicleState.actWheelDegree < myVehicleState.setWheelDegree)myVehicleState.actWheelDegree += wheelStep;
}


DWORD WINAPI sendThrd(LPVOID lpParam)
{
	SOCKET sock = *(SOCKET*)lpParam;
	char smesg[155], sChk[100], *pdata;
	int len, ret;
	clock_t start, diff;
	int msec, dmsec;
	int sec = 0;
	rxData sendData;
	char connected = 0;


	start = clock();
	diff = clock() - start;
	msec = diff * 1000 / CLOCKS_PER_SEC;
	dmsec = msec;

	do
	{
		len = 0;
		pdata = NULL;


		diff = clock() - start;
		msec = diff * 1000 / CLOCKS_PER_SEC;
		if (msec - dmsec >= 100)	// 100 msec execution
		{
			vehicleStateMachine();

			sendData.newData = 1;			// success indicator
			sendData.ignition = myVehicleState.ignition;
			sendData.gear = myVehicleState.gear;
			sendData.turnSignal = myVehicleState.turnSignal;
			sendData.speed = (int)myVehicleState.speed;
			sendData.wheelDegree = myVehicleState.actWheelDegree;
			sendData.acceleration = myVehicleState.acceleration;
			sendData.checkSum = calclChecksum(&sendData);

			sprintf(smesg, "%d,%d,%c,%d,%d,%d,%d,%d\r\n",
				sendData.newData,
				sendData.ignition,
				sendData.gear,
				sendData.turnSignal,
				sendData.speed,
				sendData.wheelDegree,
				sendData.acceleration,
				sendData.checkSum);

			/*sprintf(sChk, "%d", calclChecksum(smesg));*/

			len = strlen(smesg);
			pdata = smesg;
				
			sec++;
			printf("100msecs: %d Speed: %d Wheel: %d\r\n", sec, (int)myVehicleState.speed, myVehicleState.actWheelDegree);
			dmsec = msec;
		}


		while (len > 0)
		{
			ret = send(sock, pdata, len, 0);
			if (ret == SOCKET_ERROR)
			{
				printf("Send failed. Error: %d", WSAGetLastError());
				break;
			}
			pdata += ret;
			len -= ret;
		}

		WaitForSingleObject(hSemDisc, INFINITE);
		connected = connectedState;
		ReleaseSemaphore(hSemDisc, 1, 0);
		if (!connected)
			break;

	} while (1);

	shutdown(sock, SD_SEND);
	return 0;
}

DWORD WINAPI recvThrd(LPVOID lpParam)
{
	SOCKET sock = *(SOCKET*)lpParam;
	char smesg[256];
	int ret;
	rxData recvData;

	FILE *fp = fopen("fout.txt", "w+");

	do
	{
		ret = recv(sock, smesg, sizeof(smesg), 0);
		if (ret <= 0)
		{
			if (ret == 0)
				printf("Client disconnected\n");
			else
				printf("Connection lost! Error: %d\n", WSAGetLastError());
			break;
		}

		if (parseReceivedData(&recvData, smesg))			// parse success with checksum verified
		{
			WaitForSingleObject(hSemRXData, INFINITE);
			memcpy(&newRxData, &recvData, sizeof(rxData));
			newRxData.newData = 1;
			ReleaseSemaphore(hSemRXData, 1, 0);
		}

		printf("%.*s", ret, smesg);

		if (fp)
			fprintf(fp, "%.*s", ret, smesg);
	} while (1);

	if (fp)
		fclose(fp);

	WaitForSingleObject(hSemDisc, INFINITE);
	connectedState = 0;
	ReleaseSemaphore(hSemDisc, 1, 0);

	shutdown(sock, SD_RECEIVE);
	return 0;
}

int main()
{
	WSADATA wsa;
	SOCKET sock, newsock;
	int c;
	struct sockaddr_in server, client;
	HANDLE threads[2];

	printf("Initializing Winsock...\n");
	int ret = WSAStartup(MAKEWORD(2, 2), &wsa);
	if (ret != 0)
	{
		printf("Initialization Failed. Error: %d", ret);
		return 1;
	}
	printf("Initialized.\n");

	sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (sock == INVALID_SOCKET) {
		printf("Could not create socket! Error: %d\n", WSAGetLastError());
		return 1;
	}

	printf("Socket Created!\n");

	memset(&server, 0, sizeof(server));
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;
	server.sin_port = htons(SRV_PORT);

	//bind
	if (bind(sock, (struct sockaddr *)&server, sizeof(server)) == SOCKET_ERROR) {
		printf("Bind failed! Error: %d\n", WSAGetLastError());
		closesocket(sock);
		return 1;
	}
	printf("Binded!\n");

	// listen
	if (listen(sock, 1) == SOCKET_ERROR) {
		printf("Listen failed! Error: %d\n", WSAGetLastError());
		closesocket(sock);
		return 1;
	}
	printf("Now Listening...\n");

	while (1)
	{
		//Accept!
		c = sizeof(client);
		newsock = accept(sock, (struct sockaddr *)&client, &c);
		if (newsock == INVALID_SOCKET) {
			printf("Couldn't Accept connection! Error: %d\n", WSAGetLastError());
			closesocket(sock);
			return 1;
		}

		//char *client_ip = inet_ntoa(client.sin_addr);
		//int client_port = ntohs(client.sin_port);
		printf("Accepted Connection!\n");

		printf("Starting Reader/Writer Threads...\n");

		connectedState = 1;

		hSemRXData = CreateSemaphore(
			NULL,
			1,
			1,
			NULL);

		hSemDisc = CreateSemaphore(
			NULL,
			1,
			1,
			NULL);



		threads[0] = CreateThread(NULL, 0, sendThrd, &newsock, 0, NULL);
		threads[1] = CreateThread(NULL, 0, recvThrd, &newsock, 0, NULL);

		WaitForMultipleObjects(2, threads, TRUE, INFINITE);

		CloseHandle(threads[0]);
		CloseHandle(threads[1]);

		CloseHandle(hSemRXData);
	}

	closesocket(newsock);
	closesocket(sock);

	return 0;
}