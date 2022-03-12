from socket import *
import threading
import rospy

IP = '0.0.0.0'
PORT = 2333

BUFLEN = 512

listenSocket = socket(AF_INET,SOCK_STREAM)
listenSocket.bind((IP,PORT))

#设置最大连接数
listenSocket.listen(5)

print(f'服务端启动成功，在{PORT}端口监听客户端连接')

#accept返回两个值，dataSocket用于通讯，addr用于存储对方的IP地址
dataSocket,addr = listenSocket.accept()
print(f'接收到客户端连接：',addr)

class R(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)

    def run(self):
        while True:
            received = dataSocket.recv(BUFLEN)
            if len(received) > 0:
                info = received.decode()
                print(f'\n收到对方信息：{info}')

RThread = R()
RThread.start()

while True:
    aboutToSend = input('发送的消息>>')
    if aboutToSend =='':
        break
    dataSocket.send(aboutToSend.encode())

dataSocket.close()
listenSocket.close()
