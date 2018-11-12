#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <stdlib.h>
#include <WinSock2.h>
#include <conio.h>
#include <time.h>


#define SRV_PORT 3939

DWORD WINAPI sendThrd(LPVOID lpParam)
{
	SOCKET sock = *(SOCKET*)lpParam;
	char smesg[155], *pdata;
	int len, ret;
	clock_t start, diff;
	int msec, dmsec;
	int sec = 0;


	start = clock();
	diff = clock() - start;
	msec = diff * 1000 / CLOCKS_PER_SEC;
	dmsec = msec;

	do
	{
		diff = clock() - start;
		msec = diff * 1000 / CLOCKS_PER_SEC;
		if (msec - dmsec >= 1000)
		{
			sec++;
			printf("Seconds: %d\r\n", sec);
			dmsec = msec;
		}

		/*if (!fgets(smesg, sizeof(smesg), stdin))
			break;*/

		len = 0; strlen(smesg);
		pdata = smesg;

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
	} while (1);

	shutdown(sock, SD_SEND);
	return 0;
}

DWORD WINAPI recvThrd(LPVOID lpParam)
{
	SOCKET sock = *(SOCKET*)lpParam;
	char smesg[256];
	int ret;

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

		printf("%.*s", ret, smesg);

		if (fp)
			fprintf(fp, "%.*s", ret, smesg);
	} while (1);

	if (fp)
		fclose(fp);

	shutdown(sock, SD_RECEIVE);
	return 0;
}

int main()
{
	WSADATA wsa;
	SOCKET sock, newsock;
	int c;
	struct sockaddr_in server, client;

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
	HANDLE threads[2];
	threads[0] = CreateThread(NULL, 0, sendThrd, &newsock, 0, NULL);
	threads[1] = CreateThread(NULL, 0, recvThrd, &newsock, 0, NULL);

	WaitForMultipleObjects(2, threads, TRUE, INFINITE);

	CloseHandle(threads[0]);
	CloseHandle(threads[1]);

	closesocket(newsock);
	closesocket(sock);

	return 0;
}