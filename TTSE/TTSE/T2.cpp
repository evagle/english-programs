#include <iostream>
#include<conio.h>
#include <stdio.h>
#include <math.h>
#include <fstream>

using namespace std;
const int MAXN=100;
float matrix[MAXN][MAXN];
float vec[MAXN];
/**
矩阵的存储格式：
matrix 2 4
1 2 4 4
4 6 3 6

向量的存储格式：
vector 3
3 5 2
**/



void readMatrix(FILE *f,int &m,int &n){
	fscanf(f,"%d %d",&m,&n);
	for(int i=0;i<m;i++)
		for(int j=0;j<n;j++){
			fscanf(f,"%f",&matrix[i][j]);
		}
}


void readVector(FILE *f, int &n){
	fscanf(f,"%d",&n);
	for(int j=0;j<n;j++){
		fscanf(f,"%d",&vec[j]);
	}
}
 
 
//矩阵积和式，递归求解, 和行列式类似
float jch(float data[MAXN][MAXN],int js) 
{
	float result ,li_get;
	int i,k,h; 
	float indata[MAXN][MAXN];
	result = 0; 
	if (js == 1) result = data[0][0]; 
	else{ 
		for (i = 0; i <  js ; i ++ ) { 
			li_get = data[0][i]; 
			for(k = 0;k < js;k ++) 
				for(h =  0;h < js;h ++)
					if(k!=  0&&h!=  i)
						if(h < i) 
							indata[k - 1][h] = data[k][h];
						else 
							indata[k-1][h - 1] = data[k][h];
						result = result + li_get*jch(indata,js - 1);
		}
	}
	return result;
}

int main()
{	
	char fileNameIn[100],fileNameOut[100];
	cout<<"Input file name:"<<endl;
	cin>>fileNameIn;
	  

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
			readMatrix(pRead,mtr_m,mtr_n);
		}else{
			readVector(pRead,vec_n);
		}
	}
	fclose(pRead);
 
	
	//计算积和式,要保证mtr_m==mtr_n
	if(mtr_m==mtr_n){
		int ret=jch(matrix,mtr_m);
		ofstream fout("jiheshi.txt");
		fout<<ret<<endl;
		fout.close();
		cout<<"Result:"<<ret<<endl;
	}
	
 


	return 0;

}
