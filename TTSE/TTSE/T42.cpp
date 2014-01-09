#include <iostream>
#include<conio.h>
#include <stdio.h>
#include <string.h>
#include <fstream>

using namespace std;
struct Contact{
	char name[21];
	int age;
	char addr[21];
	char phone[14];
	char email[51];
} contacts[1000];

char fileName[100]="contacts_info.txt";

Contact token(char* src){
	char *ptr = NULL;  
	ptr = strtok(src, "$");  
	char tmp[5][51];
	int i=0;
	while (ptr != NULL)  
	{  
		strcpy(tmp[i++],ptr);
		ptr = strtok(NULL, "$");  
	} 
	Contact c;
	strcpy(c.name,tmp[0]);
	strcpy(c.addr,tmp[2]);
	strcpy(c.phone,tmp[3]);
	strcpy(c.email,tmp[4]);
	c.age = atoi(tmp[1]);
	return c;
}

int main(){
	char path[256];
	char buf[200];
	int count=0;
	int totalAge=0;
	cout<<"Enter file path:"<<endl;
	cin >> path;
	
	//ifstream fin(fileName);
	ifstream fin(path);
	while(fin>>buf){
		Contact c=token(buf);
		count++;
		totalAge+=c.age;
	}
	cout<<"There are "<<count<<" records."<<endl;
	if(count!=0)
		cout<<"Average age: "<< (totalAge/count) <<endl;
	else
		cout<<"Average age: 0"<<endl;
	fin.close();
	system("pause");
	return 0;
}