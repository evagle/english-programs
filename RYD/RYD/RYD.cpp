///�����Է�����
#include<iostream>
#include<iomanip>
#include<stdlib.h>
using namespace std; 

//----------------------------------------------ȫ�ֱ���������
const int Number=15;							//����������
double a[Number][Number],b[Number],copy_a[Number][Number],copy_b[Number];				//ϵ������ʽ
int A_y[Number];								//a[][]�����ź��������������������˳��,��a[0][0],a[1][2],a[2][1]...��A_y[]={0,2,1...};
int lenth,copy_lenth;										//���̵ĸ���
double a_sum;									//��������ʽ��ֵ
char * x;										//δ֪��a,b,c������


//----------------------------------------------����������
void input();									//���뷽����
void print_menu();								//��ӡ���˵�
int  choose ();									//����ѡ��			 
void gauss_row();								//Gauss����Ԫ�ⷽ����
void exchange_hang(int m,int n);				//�ֱ𽻻�a[][]��b[]�е�m��n����
void gauss_row_xiaoqu();						//Gauss����Ԫ��ȥ��			 
void gauss_calculate();							//����Gauss��ȥ���������δ֪����ֵ


//������
int main()
{	
	int flag=1;
	input();				//���뷽��	
	while(flag)
	{	
		print_menu();		//��ӡ���˵�

		flag=choose();		//ѡ����ʽ
	}

}


//����������
void print_menu()
{	
	cout<<"------------����ϵ���ͳ��������ʾ����:\n";
	for(int j=0;j<lenth;j++)
		cout<<"ϵ��"<<j+1<<"   ";
	cout<<"\t����";
	cout<<endl;
	for(int i=0;i<lenth;i++)
	{
		for(int j=0;j<lenth;j++)
			printf("%8d",a[i][j]);
		printf("\t%d\n",b[i]);
	}
	cout<<"	������������������������������������������������������"<<endl;
	cout<<"	��                                                  ��"<<endl;
	cout<<"	��                    ������Է���                  ��"<<endl;
	cout<<"	��                                                  ��"<<endl;
	cout<<"	��         1. Gauss����Ԫ��ȥ��                    ��"<<endl;
	cout<<"	��         2. �˳�             	                 ��"<<endl;
	cout<<"	������������������������������������������������������"<<endl;
	cout<<"��ѡ�����:"<<endl;
}

void input()
{	
	int c;
	int i,j;
	cout<<"�����ļ�·����"<<endl;
	   
	FILE *fp;
	char filename[100];
 
	cin>>filename;
 
	if((fp=fopen(filename,"r"))==NULL)
	{
		printf("�޷���juzhen.txt!\n");
		input();
	} 
	cout<<"���ı��е�һ������Ĭ��Ϊ����Ľ�"<<endl; 
	fscanf(fp,"%d",&lenth);
	printf("����Ľ�Ϊ��%d\n",lenth); 
	x=new char[lenth];	
	if(lenth>Number)
	{	
		cout<<"It is too big.\n";
		return;	
	}
	for(i=0;i<lenth;i++){
		for(j=0;j<lenth;j++)
			fscanf(fp,"%d",&a[i][j]);
		fscanf(fp,"%d",&b[i]);
	}
	fclose(fp); 
	 

	//��������
	for(i=0;i<lenth;i++)
		for(j=0;j<lenth;j++)
			copy_a[i][j]=a[i][j];
	for(i=0;i<lenth;i++)
		copy_b[i]=b[i];
	copy_lenth=lenth;
}

//����ѡ��
int choose()
{
	int choice;char ch;
	cin>>choice;
	switch(choice)
	{	
	case 2:return 0;break;
	case 1:gauss_row();break;
	  
	default:cout<<"�������,����������:";
		choose();
		break;
	}
	  
	  
	cout<<"\n\n\n";
	return 1;

}



 
 

//��˹����Ԫ������ⷽ��
void gauss_row()
{
	int i,j;
	gauss_row_xiaoqu();			//�ø�˹����Ԫ��������ϵ��������һ�������Ǿ���


	for(i=0;i<lenth;i++)
	{
		for(j=0;j<lenth;j++)
			cout<<setw(10)<<setprecision(5)<<a[i][j];
		cout<<setw(10)<<b[i]<<endl;
	}

	if(a[lenth-1][lenth-1]!=0)
	{

		cout<<"ϵ������ʽ��Ϊ��,������Ψһ�Ľ⣺\n";
		gauss_calculate();
		for(i=0;i<lenth;i++)			//������
		{	
			cout<<"x"<<i<<" = "<<b[i]<<"\n";
		}
	}
	else
		cout<<"ϵ������ʽ������,����û��Ψһ�Ľ�.\n";
}


void gauss_row_xiaoqu()			//��˹����Ԫ��ȥ��
{
	int i,j,k,maxi;double lik;
	cout<<"��Gauss����Ԫ��ȥ���������:\n";
	for(k=0;k<lenth-1;k++)
	{	
		j=k;
		for(maxi=i=k;i<lenth;i++)
			if(a[i][j]>a[maxi][j])	maxi=i;
		if(maxi!=k)	
			exchange_hang(k,maxi);//


		for(i=k+1;i<lenth;i++)
		{	
			lik=a[i][k]/a[k][k];
			for(j=k;j<lenth;j++)
				a[i][j]=a[i][j]-a[k][j]*lik;
			b[i]=b[i]-b[k]*lik;
		}
	}
}
 
 

void gauss_calculate()				//��˹��ȥ���Ժ����δ֪���Ľ��
{	
	int i,j;double sum_ax;
	b[lenth-1]=b[lenth-1]/a[lenth-1][lenth-1];
	for(i=lenth-2;i>=0;i--)
	{	
		for(j=i+1,sum_ax=0;j<lenth;j++)
			sum_ax+=a[i][j]*b[j];
		b[i]=(b[i]-sum_ax)/a[i][i];
	}
}
    

void exchange_hang(int m,int n)		//����a[][]�к�b[]����
{	
	int j;	double temp;
	for(j=0;j<lenth;j++)
	{	temp=a[m][j];
	a[m][j]=a[n][j];
	a[n][j]=temp;

	}
	temp=b[m];
	b[m]=b[n];
	b[n]=temp;
}


 
 
