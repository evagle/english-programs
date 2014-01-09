///解线性方程组
#include<iostream>
#include<iomanip>
#include<stdlib.h>
using namespace std; 

//----------------------------------------------全局变量定义区
const int Number=15;							//方程最大个数
double a[Number][Number],b[Number],copy_a[Number][Number],copy_b[Number];				//系数行列式
int A_y[Number];								//a[][]中随着横坐标增加列坐标的排列顺序,如a[0][0],a[1][2],a[2][1]...则A_y[]={0,2,1...};
int lenth,copy_lenth;										//方程的个数
double a_sum;									//计算行列式的值
char * x;										//未知量a,b,c的载体


//----------------------------------------------函数声明区
void input();									//输入方程组
void print_menu();								//打印主菜单
int  choose ();									//输入选择			 
void gauss_row();								//Gauss列主元解方程组
void exchange_hang(int m,int n);				//分别交换a[][]和b[]中的m与n两行
void gauss_row_xiaoqu();						//Gauss列主元消去法			 
void gauss_calculate();							//根据Gauss消去法结果计算未知量的值


//主函数
int main()
{	
	int flag=1;
	input();				//输入方程	
	while(flag)
	{	
		print_menu();		//打印主菜单

		flag=choose();		//选择解答方式
	}

}


//函数定义区
void print_menu()
{	
	cout<<"------------方程系数和常数矩阵表示如下:\n";
	for(int j=0;j<lenth;j++)
		cout<<"系数"<<j+1<<"   ";
	cout<<"\t常数";
	cout<<endl;
	for(int i=0;i<lenth;i++)
	{
		for(int j=0;j<lenth;j++)
			printf("%8d",a[i][j]);
		printf("\t%d\n",b[i]);
	}
	cout<<"	※※※※※※※※※※※※※※※※※※※※※※※※※※※"<<endl;
	cout<<"	※                                                  ※"<<endl;
	cout<<"	※                    求解线性方程                  ※"<<endl;
	cout<<"	※                                                  ※"<<endl;
	cout<<"	※         1. Gauss列主元消去法                    ※"<<endl;
	cout<<"	※         2. 退出             	                 ※"<<endl;
	cout<<"	※※※※※※※※※※※※※※※※※※※※※※※※※※※"<<endl;
	cout<<"请选择操作:"<<endl;
}

void input()
{	
	int c;
	int i,j;
	cout<<"输入文件路径："<<endl;
	   
	FILE *fp;
	char filename[100];
 
	cin>>filename;
 
	if((fp=fopen(filename,"r"))==NULL)
	{
		printf("无法打开juzhen.txt!\n");
		input();
	} 
	cout<<"将文本中第一个数字默认为矩阵的阶"<<endl; 
	fscanf(fp,"%d",&lenth);
	printf("矩阵的阶为：%d\n",lenth); 
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
	 

	//备份数据
	for(i=0;i<lenth;i++)
		for(j=0;j<lenth;j++)
			copy_a[i][j]=a[i][j];
	for(i=0;i<lenth;i++)
		copy_b[i]=b[i];
	copy_lenth=lenth;
}

//输入选择
int choose()
{
	int choice;char ch;
	cin>>choice;
	switch(choice)
	{	
	case 2:return 0;break;
	case 1:gauss_row();break;
	  
	default:cout<<"输入错误,请重新输入:";
		choose();
		break;
	}
	  
	  
	cout<<"\n\n\n";
	return 1;

}



 
 

//高斯列主元排列求解方程
void gauss_row()
{
	int i,j;
	gauss_row_xiaoqu();			//用高斯列主元消区法将系数矩阵变成一个上三角矩阵


	for(i=0;i<lenth;i++)
	{
		for(j=0;j<lenth;j++)
			cout<<setw(10)<<setprecision(5)<<a[i][j];
		cout<<setw(10)<<b[i]<<endl;
	}

	if(a[lenth-1][lenth-1]!=0)
	{

		cout<<"系数行列式不为零,方程有唯一的解：\n";
		gauss_calculate();
		for(i=0;i<lenth;i++)			//输出结果
		{	
			cout<<"x"<<i<<" = "<<b[i]<<"\n";
		}
	}
	else
		cout<<"系数行列式等于零,方程没有唯一的解.\n";
}


void gauss_row_xiaoqu()			//高斯列主元消去法
{
	int i,j,k,maxi;double lik;
	cout<<"用Gauss列主元消去法结果如下:\n";
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
 
 

void gauss_calculate()				//高斯消去法以后计算未知量的结果
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
    

void exchange_hang(int m,int n)		//交换a[][]中和b[]两行
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


 
 
