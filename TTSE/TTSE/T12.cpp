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
//行列式计算
int det(int a[MAXN][MAXN],int n)  
{  
	int i,j,k;
	int len;/*子行列式的阶*/
	int s=0;

	len = n-1;

	/*按照定义，初始化一个子行列式数组的空间*/
	int p[MAXN][MAXN];

	/*阶为1，按照定义计算*/
	if( 1==n ) 
		return a[0][0];

	for( k=0; k<n; k++)
	{
		for(i=0;i<len;i++)
			for(j=0;j<len;j++)
			{
				if(i<k)
					p[i][j] = a[i][j+1];/*初始化子行列式的值*/
				if(i>=k)
					p[i][j] = a[i+1][j+1];
			}
			int x=(int)pow((double)-1,k) ;
			s += x * a[k][0]* det(p,len);/*递归计算*/
	}
	return s;
}  
/*
矩阵向量乘积 
m x n的矩阵a和n x 1的向量b的乘积，结果是m x 1的向量c
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
	//写到另一个文件里面
	ofstream fout(fileNameOut);
	writeMatrix(fout,mtr_m,mtr_n);
	writeVector(fout,vec_n);
	fout.close();
	
	
	//计算行列式,要保证mtr_m==mtr_n
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
