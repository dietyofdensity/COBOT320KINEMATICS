import numpy 

def  cosangleA(A,B,C):
	return numpy.arccos((C*C+B*B-A*A)/(2*B*C))
def  cosangleB(A,B,C):
	return numpy.arccos((C*C+A*A-B*B)/(2*A*C))
def  cosangleC(A,B,C):
	return numpy.arccos((B*B+A*A-C*C)/(2*A*B))
def mm(x,y):
	return numpy.matmul(x,y)
def inv(x):
	return numpy.linalg.inv(x)
def pi():
	return numpy.pi
def atan2(y,x):
	return numpy.arctan2(y,x)
def c(x):
	return numpy.cos(x)
def s(x):
	return numpy.sin(x)
def asin(x):
	return numpy.arcsin(x)
def acos(x):
	return numpy.arccos(x)
def sqrt(x):
	return numpy.sqrt(x)
def DH(ap,a,d,th):
	# <|:^)< wizardly visit ???
	R=numpy.array([
	[c(th),	-s(th),	0,	a],
	[s(th)*c(ap) , c(th)*c(ap) , -s(ap) , -s(ap)*d],
	[s(th)*s(ap) , c(th)*s(ap) , c(ap) , c(ap)*d],
	[0 ,	0 ,	0 ,	1]
	])
	return R
def APX(a,b,c):
	h=a-b
	return ( (h > -c) & (h < c) )
def ZYZint(V1,F06):

	

	R01=DH(0,0,0,V1)
	R13=DH(-pi() / 2, 0, 0, -pi() / 2)
	R03=mm(R01,R13)
	R36=inv(R03)*F06

	R13=R36[0,2]
	R23=R36[1,2]
	R31=R36[2,0]
	R32=R36[2,1]
	R33=R36[2,2]
	R11=R36[0,0]
	R12=R36[0,1]

	beta_1=atan2(sqrt(R31*R31+R32*R32),R33)
	alpha_1=atan2(R23/s(beta_1),R13/s(beta_1))
	gamma_1=atan2(R32/s(beta_1),-R31/s(beta_1))


	beta_2=atan2(-sqrt(R31*R31+R32*R32),R33)
	alpha_2=atan2(R23/s(beta_2),R13/s(beta_2))
	gamma_2=atan2(R32/s(beta_2),-R31/s(beta_2))


	if(APX(beta_1,pi(),0.01)):
     # insert code to find shortest distance to base
		alpha_1=0
		gamma_1=atan2(-R12,R11)

	if(APX(beta_1,0,0.01)):
    
		alpha_1=0
		gamma_1=atan2(R12,-R11)

   

	if(APX(beta_2,pi(),0.01)):
     # insert code to find shortest distance to base
		alpha_2=0
		gamma_2=atan2(-R12,R11)

	if(APX(beta_2,0,0.01)):
		alpha_2=0
		gamma_2=atan2(R12,-R11)
	
	ang1 = [alpha_1,beta_1,gamma_1]
	ang2 = [alpha_2,beta_2,gamma_2]
	return [ang1,ang2]
def CfromZYZ(V1,a,b,y,F06):
	FPT4 = DH( 0  ,   0       ,  88.78          ,   a + pi()/2       )
	F4T5 = DH( pi()/2     ,   0       ,   95          ,   -b      )
	F5T6 = DH( -pi()/2    ,   0       ,   0           ,   y      )

	FP6=mm(mm(FPT4,F4T5),F5T6)
	F04p=mm(F06,inv(FP6))
	Vec4=mm(F04p,[0,0,0,1])
	X4=Vec4[0]
	Y4=Vec4[1]
	Z4=Vec4[2]
	Vecxy=[X4,Y4,0,1]

	

	XYlength=(sqrt(X4*X4+Y4*Y4))
	XYZlengt=(sqrt(X4*X4+Y4*Y4+Z4*Z4))

	L2=135
	L3=120

	M01=DH(0,0,0,V1)
	vrot=mm(M01,[1,0,0,0])
	vths=Vecxy/XYZlengt
	direction = mm(vrot,vths)

	dir=0
	if(direction>0):
		dir=-1
	else:
		dir=1


	alpha=atan2(Z4,XYlength*dir)-pi()/2

	

	A=cosangleA(L2,L3,XYZlengt)
	B=cosangleB(L2,L3,XYZlengt)
	C=cosangleC(L2,L3,XYZlengt)



	V2_1 = alpha - B
	V2_2 = alpha + B

	V3_1 = pi() - C
	V3_2 = -pi() + C

	V4_1 = a - (pi() / 2 + alpha + A)
	V4_2 = a - (pi() / 2 + alpha - A)

	V5=-b
	V6=y
	invalid=XYZlengt-255

	S1=[invalid,(rtd(V1)),(rtd(V2_1)),(rtd(V3_1)),(rtd(V4_1)),(rtd(V5)),(rtd(V6))]

	S2=[invalid,(rtd(V1)),(rtd(V2_2)),(rtd(V3_2)),(rtd(V4_2)),(rtd(V5)),(rtd(V6))]
	
	return [S1,S2]
def matAPX(A,B):
	ret = True
	#rotation
	ths=APX(A[0][0],B[0][0],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[0][1],B[0][1],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[0][2],B[0][2],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[1][0],B[1][0],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
		ths=APX(A[1][1],B[1][1],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[1][2],B[1][2],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[2][0],B[2][0],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[2][1],B[2][1],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[2][2],B[2][2],0.05)
	if(ths & ret):
		ret=ths
	else:
		ret=False

	#position
	ths=APX(A[0][3],B[0][3],30)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[1][3],B[1][3],30)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	ths=APX(A[2][3],B[2][3],30)
	if(ths & ret):
		ret=ths
	else:
		ret=False
	return ret
def rtd(x):
	return x*180/numpy.pi
def dtr(x):
	return x*numpy.pi/180
def Via(ANGS,FBE):

	
	VIABLES=0
	first=true
	for R in range(16):
		RET=True
		if(TEST2(R,1)<1):
       
			for K in range(6):
				A=0
				B=true
				a=TEST2[R,K+1]
				if(K==6):
					A = setinrange(a,180)
					B = APX(A,0,175+1)
				else:
					A=setinrange(a,180)
					B= APX(A,0,165+1)
            
            
				TEST2[R,K+1]=A
				if(B==false):
					RET=false
		if(RET):
			if(first):
				VIABLES=TEST2[R,1:7]
				first=false
			else:
				VIABLES=[VIABLES,TEST2[R,1:7]]
	return VIABLES
def ikin(FBE):

	VK=mm(FBE,[[0],[0],[0],[1]])
	XE=VK[0]
	YE=VK[1]
	ZE=VK[2]
	
	FE6=DH(0,0,-65.5,0)
	F0B=DH(0,0,-173.9,0)
	F06=mm(mm(F0B,FBE),FE6)
	VK=mm(F06,numpy.array([0,0,0,1]))
	print(f"vk is x: {VK[0]} y: {VK[1]}z: {VK[2]}")
	X6=VK[0]
	Y6=VK[1]
	Z6=VK[2]
	# test here
	# disp("test endpoints");
	# disp(FBE*vector4(0,0,0));
	# disp(F06*vector4(0,0,0));
	# calculate angle
	a=atan2(Y6,X6)
	xyPlanedist=sqrt(X6*X6+Y6*Y6)
	b=asin(88.78/xyPlanedist)
	V1_1=a-b
	V1_2=a+b
	V1_3=V1_1+pi()
	V1_4=V1_2+pi()

	RK1=ZYZint(V1_1,F06)
	S1S=CfromZYZ(V1_1,RK1[0][0],RK1[0][1],RK1[0][2],F06)
	S2S=CfromZYZ(V1_1,RK1[1][0],RK1[1][1],RK1[1][2],F06)

	RK2=ZYZint(V1_2,F06)
	S3S=CfromZYZ(V1_2,RK2[0][0],RK2[0][1],RK2[0][2],F06)
	S4S=CfromZYZ(V1_2,RK2[1][0],RK2[1][1],RK2[1][2],F06)

	RK3=ZYZint(V1_3,F06)
	S5S=CfromZYZ(V1_3,RK3[0][0],RK3[0][1],RK3[0][2],F06)
	S6S=CfromZYZ(V1_3,RK3[1][0],RK3[1][1],RK3[1][2],F06)

	RK4=ZYZint(V1_4,F06)
	S7S=CfromZYZ(V1_4,RK4[0][0],RK4[0][1],RK4[0][2],F06)
	S8S=CfromZYZ(V1_4,RK4[1][0],RK4[1][1],RK4[1][2],F06)


	T=[S1S[0],S1S[1],S2S[0],S2S[1],S3S[0],S3S[1],S4S[0],S4S[1],S5S[0],S5S[1],S6S[0],S6S[1],S7S[0],S7S[1],S8S[0],S8S[1]]
	return T
def FWDKIN2(a1,a2,a3,a4,a5,a6):

	dr=pi()/180
	FBT0 = DH( 0    ,   0       ,   173.9           , 0     )
	F0T1 = DH( 0        ,   0       ,   0           ,   a1*dr       )
	F1T2 = DH( -pi()/2    ,   0       ,   0           ,   a2*dr - pi()/2 )
	F2T3 = DH( 0        ,   135     ,   0           ,   a3*dr        )
	F3T4 = DH( 0        ,   120     ,   88.78       ,   a4*dr + pi()/2 )
	F4T5 = DH( pi()/2     ,   0       ,   95          ,   a5*dr        )
	F5T6 = DH( -pi()/2    ,   0       ,   0           ,   a6*dr        )
	F6TE = DH( 0        ,   0       , 65.5          ,   0    )

	RET =mm(mm( mm(FBT0, F0T1),mm( F1T2 , F2T3 )), mm(mm( F3T4 , F4T5) , mm( F5T6 , F6TE)))

	return RET
def main():
	A=DH(0, 173.9, 0, 0)
	B=DH(0, 0, 0, 0)
	C=mm(A,B)
	for i in C:
		print(i)
	print(f"the {cosangleA(1, 2, 2)}")
	FK=FWDKIN2(10,20,30,40,50,60)
	for i in FK:
		print(i)

	SOL=ikin(FK)
	for i in SOL:
		print(i)
	


	return
def funktest():

	print(str(rtd(cosangleA(3, 3, 3)))+"should be 60")
	print(str(rtd(cosangleB(3, 3, 3)))+"should be 60")
	print(str(rtd(cosangleC(3, 3, 3)))+"should be 60")

	A=numpy.array([
	[2,10,18,26],
	[4,12,20,28],
	[6,5,22,30],
	[8,16,5,32]
	])

	B=numpy.array([
	[1,2,3,4],
	[5,6,7,8],
	[9,10,11,12],
	[13,14,15,16]
	])

	C=mm(A,B)
	print(C)
	print(str(APX(3, 1, 2.5))+"should be true")
	print(str(APX(-3, 1, 2.5))+"should be false")
	print(str(APX(3, -1, 2.5))+"should be false")
	print(str(APX(-3, -1, 2.5))+"should be true")


	FK=FWDKIN2(60, 90, 0, 0, 40, 0)
	print(FK)
	R=ZYZint(60, FK)
	print(R.*pi()/180)






	return 0
funktest()

