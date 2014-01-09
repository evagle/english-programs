#include <iostream>
#include<conio.h>
#include <stdio.h>
#include <math.h>
#include <fstream>


using namespace std;
const int MAXN=100;
int matrix[MAXN][MAXN];
int vec[MAXN];
/**
����Ĵ洢��ʽ��
matrix 2 4
1 2 4 4
4 6 3 6

�����Ĵ洢��ʽ��
vector 3
3 5 2
**/


void readMatrix(FILE *f,int &m,int &n){
	fscanf(f,"%d %d",&m,&n);
	for(int i=0;i<m;i++)
		for(int j=0;j<n;j++){
			fscanf(f,"%d",&matrix[i][j]);
		}
}

void readVector(FILE *f, int &n){
	fscanf(f,"%d",&n);
	for(int j=0;j<n;j++){
		fscanf(f,"%d",&vec[j]);
	}
}

void writeVector(ofstream &fout,int n){
	fout<<"vector "<<n<<endl;
	for(int j=0;j<n;j++){
		fout<<vec[j]<<" ";
	}
	fout<<endl;
}
void writeMatrix(ofstream &fout,int m,int n){
	fout<<"matrix "<<m<<n;
	for(int i=0;i<m;i++){
		for(int j=0;j<n;j++){
			fout<<matrix[i][j]<<" ";
		}
		fout<<endl;
	}
}
//����ʽ����
int det(int a[MAXN][MAXN],int n)  
{  
	int i,j,k;
	int len;/*������ʽ�Ľ�*/
	int s=0;

	len = n-1;

	/*���ն��壬��ʼ��һ��������ʽ����Ŀռ�*/
	int p[MAXN][MAXN];

	/*��Ϊ1�����ն������*/
	if( 1==n ) 
		return a[0][0];

	for( k=0; k<n; k++)
	{
		for(i=0;i<len;i++)
			for(j=0;j<len;j++)
			{
				if(i<k)
					p[i][j] = a[i][j+1];/*��ʼ��������ʽ��ֵ*/
				if(i>=k)
					p[i][j] = a[i+1][j+1];
			}
			int x=(int)pow((double)-1,k) ;
			s += x * a[k][0]* det(p,len);/*�ݹ����*/
	}
	return s;
}  
/*
���������˻� 
m x n�ľ���a��n x 1������b�ĳ˻��������m x 1������c
*/
void mutiple(int a[MAXN][MAXN],int b[MAXN],int c[MAXN],int m,int n){
	for(int i=0;i<m;i++){
		c[i]=0;
		for(int j=0;j<n;j++){
			c[i]+=a[i][j]*b[j];
		}
	}
}


int main()
{	

	char fileNameIn[100],fileNameOut[100];
	cout<<"Input file name:"<<endl;
	cin>>fileNameIn;
	cout<<"Output file name:"<<endl;
	cin>>fileNameOut;

	char type[10];
	int vec_n,mtr_m,mtr_n;

	FILE *pRead=fopen(fileNameIn,"r");
	if(NULL == pRead)
	{
		cout<<"�ļ�����"<<endl;
		return 0;
	}
	//���ļ��ж�ȡ����
	while(fscanf(pRead,"%s",type)>0)
	{
		if(!strcmp(type,"martix")){
			readMatrix(pRead,mtr_m,mtr_n);
		}else{
			readVector(pRead,vec_n);
		}
	}
	fclose(pRead);
	//д����һ���ļ�����
	ofstream fout(fileNameOut);
	writeMatrix(fout,mtr_m,mtr_n);
	writeVector(fout,vec_n);
	fout.close();
	
	
	//��������ʽ,Ҫ��֤mtr_m==mtr_n
	if(mtr_m==mtr_n){
		int ret=det(matrix,mtr_m);
		ofstream fout("det.txt");
		fout<<ret<<endl;
		fout.close();
	}
	
	if(mtr_n==vec_n){
		int c[MAXN];
		mutiple(matrix,vec,c,mtr_m,mtr_n);
		ofstream fout("matrix-mutiply-vector.txt");
		for(int i=0;i<mtr_m;i++)
			fout<<c[i]<<"\t";
		fout.close();
	}
	/*
	if(mtr_m==mtr_n&&mtr_n==vec_n){
		int x[MAXN];
		gauss(matrix,vec,x,mtr_m);
		ofstream fout("gauss.txt");
		for(int i=0;i<mtr_m;i++)
			fout<<x[i]<<"\t";
		fout.close();
	}*/

	return 0;

}
