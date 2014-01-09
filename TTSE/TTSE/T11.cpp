#include <iostream>
#include<conio.h>
#include <stdio.h>
#include <math.h>
#include <fstream>

using namespace std;

const int MAXN=500;
int matrix[MAXN][MAXN];
int vec[MAXN];
/**
矩阵的存储格式：
matrix 2 4
1 2 4 4
4 6 3 6

向量的存储格式：
vector 3
3 5 2
**/


void readMatrix(FILE *f,int *m,int *n){
	fscanf(f,"%d %d",m,n);
	for(int i=0;i<*m;i++)
		for(int j=0;j<*n;j++){
			fscanf(f,"%d",&matrix[i][j]);
		}
}

void readVector(FILE *f, int* n){
	fscanf(f,"%d",n);
	for(int j=0;j<*n;j++){
		fscanf(f,"%d",&vec[j]);
	}
}
void writeVector(ofstream fout,int n){
	fout<<"vector "<<n<<endl;
	for(int j=0;j<n;j++){
		fout<<vec[j]<<" ";
	}
	fout<<endl;
}
void writeMatrix(ofstream fout,int m,int n){
	fout<<"matrix "<<m<<n;
	for(int i=0;i<m;i++){
		for(int j=0;j<n;j++){
			fout<<matrix[i][j]<<" ";
		}
		fout<<endl;
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
		cout<<"文件错误"<<endl;
        return 0;
    }
    //从文件中读取数据
	while(fscanf(pRead,"%s",type)>0)
	{
		if(!strcmp(type,"martix")){
			readMatrix(pRead,&mtr_m,&mtr_n);
		}else{
			readVector(pRead,&vec_n);
		}
	}
	fclose(pRead);
	//写到另一个文件里面
	ofstream fout(fileNameOut);
	writeMatrix(fout,mtr_m,mtr_n);
	writeVector(fout,vec_n);
	fout.close();



	return 0;

}
