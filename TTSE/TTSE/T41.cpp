#include <iostream>
#include<conio.h>
#include <stdio.h>
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
ofstream fout(fileName);
int contactNum=0;
void printMenu(){
	cout<<"请选择一下操作选项："<<endl;
	cout<<"A．向文件中增加记录"<<endl;
	cout<<"B．显示文件中的所有记录"<<endl;
	cout<<"C．修改任意一个记录"<<endl;
	cout<<"D．按照姓名查找一个同学的记录"<<endl;
	cout<<"E．删除某个同学的记录"<<endl;
	cout<<"S．保存到文件"<<endl;
	cout<<"Q．退出"<<endl;
}
void printOneRecords(Contact c){
	cout<<c.name<<"\t"<<c.age<<"\t"<<c.addr<<"\t"<<c.phone<<"\t"<<c.email<<endl;
}
void addRecord(){
	Contact tmp;
	cout<<"Enter Name :";
	cin>>tmp.name;
	cout<<"Enter age :";
	cin>>tmp.age;
	if(tmp.age<0||tmp.age>200){
		cout<<"Input Error: Age should be in range (0,200)"<<endl;
		return;
	}
	cout<<"Enter address :";
	cin>>tmp.addr;
	cout<<"Enter phone :";
	cin>>tmp.phone;
	cout<<"Enter email :";
	cin>>tmp.email;	

	cout<<"Information entered :";
	printOneRecords(tmp);
	cout<<endl;
	 
	contacts[contactNum++]=tmp;
}

void showAllRecords(){
	if(contactNum==0)
		cout<<"There is no record now."<<endl;
	else
		cout<<"Name\tAge\tAddress\tPhone\tEmail"<<endl;
	for(int i=0;i<contactNum;i++){
		Contact tmp=contacts[i];
		printOneRecords(tmp);
	}
}
void modifyRecord(){
	cout<<"Enter the name of which you wanna to modify:"<<endl;
	char tmpName[21];
	cin>>tmpName;

	Contact tmp;
	strcpy(tmp.name,tmpName);
	cout<<"Enter age :";
	cin>>tmp.age;
	cout<<"Enter address :";
	cin>>tmp.addr;
	cout<<"Enter phone :";
	cin>>tmp.phone;
	cout<<"Enter email :";
	cin>>tmp.email;
	cout<<"Information has been modified :";
	printOneRecords(tmp);
	cout<<endl;
	 
	for(int i=0;i<contactNum;i++){
		if(!strcmp(contacts[i].name,tmpName)){
			contacts[i]=tmp;
		}
	}
}
void searchByName(){
	cout<<"Enter the name of which you wanna to search:"<<endl;
	char tmpName[21];
	cin>>tmpName;
	for(int i=0;i<contactNum;i++){
		if(!strcmp(contacts[i].name,tmpName)){
			printOneRecords(contacts[i]);
		}
	}

}
void deleteRecord(){
	cout<<"Enter the name of which you wanna to delete:"<<endl;
	char tmpName[21];
	cin>>tmpName;
	for(int i=0;i<contactNum;i++){
		if(!strcmp(contacts[i].name,tmpName)){
			for(int j=i;j<contactNum-1;j++)
				contacts[j]=contacts[j+1];
			cout<<"Delete successfully"<<endl;
			contactNum--;
		}
	}
	
}
void writeToFile(){
	for(int i=0;i<contactNum;i++){
		Contact tmp=contacts[i];
		fout<<tmp.name<<"$"<<tmp.age<<"$"<<tmp.addr<<"$"<<tmp.phone<<"$"<<tmp.email<<endl;
	}
	cout<<"保存成功"<<endl;
}
int main()
{	
	printMenu();
	
	while(1)
	{	
		char c= getchar();
		if(c=='\n')
			c= getchar();
		if(c=='A'||c=='a'){
			addRecord();
		}else if(c=='B'||c=='b'){
			showAllRecords();
		}else if(c=='C'||c=='c'){
			modifyRecord();
		}else if(c=='D'||c=='d'){
			searchByName();
		}else if(c=='E'||c=='e'){
			deleteRecord();
		}else if(c=='S'||c=='s'){
			writeToFile();
		}else if(c=='Q'||c=='q'){
			break;
		}
		printMenu();

	}
	writeToFile();
	fout.close();
	system("pause");
	return 0;

}
