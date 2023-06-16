function T=compute2_from_zyz(V1,a,b,y,F06)

FPT4 = DH( 0  ,   0       ,  88.78          ,   a + pi/2       );

F4T5 = DH( pi/2     ,   0       ,   95          ,   -b      );

F5T6 = DH( -pi/2    ,   0       ,   0           ,   y      );

FP6=FPT4*F4T5*F5T6;
F04p=F06*inv(FP6);
Vec4=F04p*vector4(0,0,0);
X4=Vec4(1);
Y4=Vec4(2);
Z4=Vec4(3);
Vecxy=[X4;Y4;0;1];

%disp("v4 " +Vec4);

XYlength=real(sqrt(X4^2+Y4^2));
XYZlengt=real(sqrt(X4^2+Y4^2+Z4^2));

L2=135.002;
L3=120.002;

M01=DH(0,0,0,V1);
vrot=M01*vector4(1,0,0);
vths=Vecxy./XYZlengt;
direction = vrot'*vths;

%disp(vrot);
%disp(vths);

dir=0;
if(direction>0)
dir=-1;
else
dir=1;
end

alpha=atan2(Z4,XYlength*dir)-pi/2;

A=cosangleA(L2,L3,XYZlengt);
B=cosangleB(L2,L3,XYZlengt);
C=cosangleC(L2,L3,XYZlengt);

%disp("length "+XYZlengt);

 V2_1 = alpha - B;
 V2_2 = alpha + B;

 V3_1 = pi - C;
 V3_2 = -pi + C;

 V4_1 = a - (pi / 2 + alpha + A);
 V4_2 = a - (pi / 2 + alpha - A);

V5=-b;
V6=y;
invalid=XYZlengt-255;

S1=[invalid,real(rtd(V1)) ,real(rtd(V2_1)) ,real(rtd(V3_1)) ,real(rtd(V4_1)) ,real(rtd(V5)) ,real(rtd(V6))];

S2=[invalid,real(rtd(V1) ),real(rtd(V2_2)) ,real(rtd(V3_2)),real(rtd(V4_2)) ,real(rtd(V5) ),real(rtd(V6))];

T=[S1;S2];
end
