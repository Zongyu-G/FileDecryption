/*************************************************************************
	> File Name: FileDecryption.c
	> Author: Zongyu Guo
	> Mail: 1850441981@qq.com 
	> Created Time: Wed Mar  9 13:58:49 2022
 ************************************************************************/

#include "FileDecryption.h"
#include <string.h>

int main(int argc, char *argv[])
{
	if (argc == 3)
	{
		char *rpath = argv[1];
		char *wpath = argv[2];
		ListByte *EncryptData = (ListByte *)malloc(sizeof(ListByte));
		ListByte *DecryptData = (ListByte *)malloc(sizeof(ListByte));
		if (!ReadFile(rpath, EncryptData))
			return 0;
		FileDecrytion(EncryptData, DecryptData);
		if (!WriteFile(wpath, DecryptData))
			return 0;
		return 1;
	}
	else if (argc == 2)
	{
		char *rpath = argv[1];
		ListByte *EncryptData = (ListByte *)malloc(sizeof(ListByte));
		ListByte *DecryptData = (ListByte *)malloc(sizeof(ListByte));
		if (!ReadFile(rpath, EncryptData))
		{
			system("pause");
			return 0;
		}
		if (!ReadFile(rpath, DecryptData))
		{
			system("pause");
			return 0;
		}
		// ShowFileData1(EncryptData,DecryptData);
		FileDecrytion(EncryptData, DecryptData);
		char *wpath = strcat(rpath, ".out");
		if (!WriteFile(wpath, DecryptData))
		{
			system("pause");
			return 0;
		}
		system("pause");
		return 1;
	}
	else
	{
		printf("Usage: 程序名 加密文件路径 解密文件保存路径。\r\nExample: ./FileDecryption ./1.txt ./11.txt\r\n");
		system("pause");
		return 0;
	}
}

//将文件数据读取于链表中
int ReadFile(char *path, ListByte *data)
{
	FILE *fp = NULL;
	fp = fopen(path, "rb");
	if (fp == NULL)
	{
		printf("Fail to open file!\r\n");
		return 0;
	}
	size_t r = 0;
	ListByte *tail;
	tail = data;
	tail->next = NULL;
	while (1)
	{
		byte buffer[1024];
		r = fread(buffer, sizeof(byte), 1024, fp);
		for (size_t i = 0; i < r; i++)
		{
			ListByte *temp = (ListByte *)malloc(sizeof(ListByte));
			temp->element = buffer[i];
			tail->next = temp;
			temp->next = NULL;
			tail = temp;
		}
		if (r < 1024)
			break;
	}
	fclose(fp);
	printf("Reading file success!\r\n");
	return 1;
}

//将链表数据写入文件
int WriteFile(char *path, ListByte *data)
{
	FILE *fp = NULL;
	fp = fopen(path, "wb");
	if (fp == NULL)
	{
		printf("Fail to open file!\r\n");
		return 0;
	}
	ListByte *tail;
	tail = data->next;
	while (tail != NULL)
	{
		fputc(tail->element, fp);
		tail = tail->next;
	}
	fclose(fp);
	printf("Writing file success!\r\n");
	return 1;
}

//解密链表数据
int FileDecrytion(ListByte *EncryptData, ListByte *DecryptData)
{
	ListByte *t = EncryptData->next;
	ListByte *t0;
	t0 = DecryptData;
	t0->next = NULL;
	size_t r = 0;
	size_t i = 0;
	while (t != NULL)
	{
		if (r >= 512)
		{
			if (i == 256)
				i = 0;
			ListByte *temp = (ListByte *)malloc(sizeof(ListByte));
			temp->element = t->element ^ keys[i];
			t0->next = temp;
			temp->next = NULL;
			t0 = temp;
			i++;
		}
		t = t->next;
		r++;
	}
	printf("Decrytion Success!\r\n");
	return 0;
}

//显示链表数据
void ShowFileData(ListByte *data)
{
	ListByte *t = data->next;
	while (t != NULL)
	{
		printf("%d ", t->element);
		t = t->next;
	}
}

void ShowFileData1(ListByte *data,ListByte *data1)
{
	ListByte *t = data->next;
	ListByte *t1 = data1->next;
	while (t != NULL)
	{
		printf("%d %d\r\n", t->element,t1->element);
		t = t->next;
		t1 = t1->next;
	}
}