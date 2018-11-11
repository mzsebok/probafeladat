#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <WinSock2.h>


#define SRVPORT		9876
#define RXBUFSIZE	128   /* Receive buffer size */
#define MAXPENDING	1     /* Maximum outstanding connection requests */


void DieWithError(char *errorMessage);
int CreateTCPServerSocket(unsigned short port);
void HandleTCPClient(int clntSocket);   /* TCP client handling function */


void main(int argc, char *argv[])
{
	WSADATA wsaData;					/* Structure for WinSock setup communication */
	SOCKET listenSocket;				/* Listening socket */
	int retval;
	int clntLen;
	struct sockaddr_in echoClntAddr;	/* Client address */
	int recvMsgSize;					/* Size of received message */



	/* Request Winsock version 2.2 */

	if ((retval = WSAStartup(0x202, &wsaData)) != 0)
	{
		fprintf(stderr, "Server: WSAStartup() failed with error %d\n", retval);
		WSACleanup();
		exit(1);
	}
	else
		printf("Server: WSAStartup() is OK.\n");

	listenSocket = CreateTCPServerSocket(SRVPORT);


	for (;;) /* Run forever */
	{
		/* Set the size of the in-out parameter */
		clntLen = sizeof(echoClntAddr);

		/* Wait for a client to connect */
		if ((listenSocket = accept(listenSocket, (struct sockaddr *) &echoClntAddr, &clntLen)) < 0)
			DieWithError("accept() failed");

		/* clntSock is connected to a client! */

		printf("Handling client %s\n", inet_ntoa(echoClntAddr.sin_addr));

		HandleTCPClient(listenSocket);
	}
}


void DieWithError(char *errorMessage)
{
	fprintf(stderr, "%s: %d\n", errorMessage, WSAGetLastError());
	exit(1);
}

int CreateTCPServerSocket(unsigned short port)
{
	int sock;                        /* socket to create */
	struct sockaddr_in echoServAddr; /* Local address */

	/* Create socket for incoming connections */
	if ((sock = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP)) < 0)
		DieWithError("socket() failed");

	/* Construct local address structure */
	memset(&echoServAddr, 0, sizeof(echoServAddr));   /* Zero out structure */
	echoServAddr.sin_family = AF_INET;                /* Internet address family */
	echoServAddr.sin_addr.s_addr = htonl(INADDR_ANY); /* Any incoming interface */
	echoServAddr.sin_port = htons(port);			  /* Local port */

	/* Bind to the local address */
	if (bind(sock, (struct sockaddr *) &echoServAddr, sizeof(echoServAddr)) < 0)
		DieWithError("bind() failed");

	/* Mark the socket so it will listen for incoming connections */
	if (listen(sock, MAXPENDING) < 0)
		DieWithError("listen() failed");

	return sock;
}

void HandleTCPClient(int clntSocket)
{
	char rxBuffer[RXBUFSIZE];			/* Buffer for echo string */
	int recvMsgSize;                    /* Size of received message */

	/* Receive message from client */
	if ((recvMsgSize = recv(clntSocket, rxBuffer, RXBUFSIZE, 0)) < 0)
		DieWithError("recv() failed");

	/* Send received string and receive again until end of transmission */
	while (recvMsgSize > 0)      /* zero indicates end of transmission */
	{
		printf("%.*s\r\n", recvMsgSize, rxBuffer);

		/* Echo message back to client */
		if (send(clntSocket, rxBuffer, recvMsgSize, 0) != recvMsgSize)
			DieWithError("send() failed");

		/* See if there is more data to receive */
		if ((recvMsgSize = recv(clntSocket, rxBuffer, RXBUFSIZE, 0)) < 0)
			DieWithError("recv() failed");
	}

	closesocket(clntSocket);    /* Close client socket */
}