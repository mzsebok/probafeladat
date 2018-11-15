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

HANDLE hSemRXData;		// semaphore for received data
HANDLE hSemDisc;		// semaphore for connected state
HANDLE hSemClk;			// semaphore for sent time

typedef struct {
	int   ignition;
	char  gear;
	int   turnSignal;
	float speed;
	int   setWheelDegree;
	int   actWheelDegree;
	int   acceleration;
	int   lastSuccess;
}vehicleState;

vehicleState myVehicleState = { 0, 'N', 0, 0, 0, 0 , 0, 0};		// actual vehicle state

typedef struct {
	int  newData;
	int  ignition;
	char gear;
	int  turnSignal;
	int  speed;
	int  wheelDegree;
	int  acceleration;
	int  checkSum;
}rxData;

rxData newRxData = {0, 0, 'N', 0, 0, 0, 0, 0 };					// store received data
//rxData newRxData = {0, 0, 'D', 0, 0, 320, 50, 0 };

char connectedState = 0;
clock_t sentTime;				// time stamp for the last packet sent


int calclChecksum(char *sData)
{
	int i;
	int chkSum = 0;

	i = strlen(sData) - 1;

	while (*(sData+i) != ',' && i > 0)i--;		// find last parameter before checksum

	while ((i--) != 0)
	{
		chkSum += *(sData+i);
	}

	return (chkSum);
}

int parseReceivedData(rxData *recvData, char *smsg)
{
	recvData->newData = 0;
	

	sscanf(smsg, "%d,%c,%d,%d,%d,%d,%d\n",
		&recvData->ignition,
		&recvData->gear,
		&recvData->turnSignal,
		&recvData->speed,
		&recvData->wheelDegree,
		&recvData->acceleration,
		&recvData->checkSum);

	if (calclChecksum(smsg) == recvData->checkSum)
		return (1);
	else
		return (0);
}

void vehicleStateMachine(vehicleState * vHState)
{
	int wheelStep = WHEELSTEPVAL;

	WaitForSingleObject(hSemRXData, INFINITE);

	vHState->lastSuccess = 0;

	if (newRxData.newData)
	{
		vHState->ignition = newRxData.ignition;
		vHState->gear = newRxData.gear;
		vHState->turnSignal = newRxData.turnSignal;
		vHState->acceleration = newRxData.acceleration;
		vHState->setWheelDegree = newRxData.wheelDegree;
		newRxData.newData = 0;
		vHState->lastSuccess = 1;
	}

	ReleaseSemaphore(hSemRXData, 1, 0);


	if(vHState->gear != 'N')
		vHState->speed += ((float)vHState->acceleration * (float)5.0) / (float)100.0;


	if (vHState->gear == 'D' && vHState->speed > MAXSPEED_D)vHState->speed = MAXSPEED_D;
	if (vHState->gear == 'R' && vHState->speed > MAXSPEED_R)vHState->speed = MAXSPEED_R;
	if (vHState->speed < 0)vHState->speed = 0;


	if ((wheelStep = abs(vHState->actWheelDegree - vHState->setWheelDegree)) > WHEELSTEPVAL)wheelStep = WHEELSTEPVAL;

	if (vHState->actWheelDegree > vHState->setWheelDegree)vHState->actWheelDegree -= wheelStep;
	else if(vHState->actWheelDegree < vHState->setWheelDegree)vHState->actWheelDegree += wheelStep;
}


DWORD WINAPI sendThrd(LPVOID lpParam)
{
	SOCKET sock = *(SOCKET*)lpParam;
	char smesg[155], *pdata;
	int len, ret;
	clock_t start, diff;
	int msec, dmsec;
	int msecCount = 0;
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
			vehicleStateMachine(&myVehicleState);

			sendData.newData = myVehicleState.lastSuccess;			// success indicator
			sendData.ignition = myVehicleState.ignition;
			sendData.gear = myVehicleState.gear;
			sendData.turnSignal = myVehicleState.turnSignal;
			sendData.speed = (int)myVehicleState.speed;
			sendData.wheelDegree = myVehicleState.actWheelDegree;
			sendData.acceleration = myVehicleState.acceleration;
			sendData.checkSum = 0;

			sprintf(smesg, "%d,%d,%c,%d,%d,%d,%d,%d\n",
				sendData.newData,
				sendData.ignition,
				sendData.gear,
				sendData.turnSignal,
				sendData.speed,
				sendData.wheelDegree,
				sendData.acceleration,
				sendData.checkSum);

			sendData.checkSum = calclChecksum(smesg);

			sprintf(smesg, "%d,%d,%c,%d,%d,%d,%d,%d\n",
				sendData.newData,
				sendData.ignition,
				sendData.gear,
				sendData.turnSignal,
				sendData.speed,
				sendData.wheelDegree,
				sendData.acceleration,
				sendData.checkSum);

			len = strlen(smesg);
			pdata = smesg;
				
			msecCount++;

			printf("100msecs: %d \t Ign: %d Gear: %c Acc: %d \t Speed: %d \t Wheel: %d \t Wheel Setp: %d \t Turn: %d \n",
				msecCount,
				myVehicleState.ignition,
				myVehicleState.gear,
				myVehicleState.acceleration,
				(int)myVehicleState.speed,
				myVehicleState.actWheelDegree,
				myVehicleState.setWheelDegree,
				myVehicleState.turnSignal);


			dmsec = msec;
		}


		while (len > 0)
		{
			ret = send(sock, pdata, len, 0);
			if (ret == SOCKET_ERROR)
			{
				printf("Send failed. Error: %d \n", WSAGetLastError());
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

		WaitForSingleObject(hSemClk, INFINITE);
		sentTime = clock();
		ReleaseSemaphore(hSemClk, 1, 0);

	} while (1);

	shutdown(sock, SD_SEND);
	return 0;
}

DWORD WINAPI recvThrd(LPVOID lpParam)
{
	SOCKET sock = *(SOCKET*)lpParam;
	char smesg[256];
	char timeStr[256];
	int ret;
	rxData recvData;
	time_t rawtime;
	struct tm * timeinfo;
	int timeoutVal;


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

			WaitForSingleObject(hSemClk, INFINITE);			// detect if packet received in time
			timeoutVal = (clock() - sentTime) * 1000 / CLOCKS_PER_SEC;
			if (timeoutVal > 1)	// > 100ms
			{
				printf("Packet timeout: %dms\n", timeoutVal);
			}
			ReleaseSemaphore(hSemClk, 1, 0);
		}

		time(&rawtime);
		timeinfo = localtime(&rawtime);
		sprintf(timeStr, "%s", asctime(timeinfo));
		timeStr[strlen(timeStr)-1] = '\0';

		printf("[%s] ", timeStr);
		printf("%.*s\n", ret, smesg);

		if (fp)
		{
			fprintf(fp, "[%s] ", timeStr);
			fprintf(fp, "%.*s", ret, smesg);
		}
	
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