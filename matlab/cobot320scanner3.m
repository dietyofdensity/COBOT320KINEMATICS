
%% Start up rutine

clc
close
clear

% global robot

% Generate a Robolink object RDK. This object interfaces with RoboDK.
RDK = Robolink;

% Display the list of all items in the main tree
fprintf('Available items in the station:\n');
disp(RDK.ItemList());

robot = RDK.ItemUserPick('Select one robot', RDK.ITEM_TYPE_ROBOT);

if robot.Valid() == 0
    error('No robot selected');
end

%% jump to a location Set the home joints
jhome = [ -50, -110, 100, 30, 70, -50];

% Set the robot at the home position
robot.setJoints(jhome);

%% Move to a location


bT0= robot.Pose();

bT1 = bT0;
bT1(1,4) = bT0(1,4) - 100;
robot.MoveL(bT1);

bTt=[-1 0 0 500;
     0  1 0 200;
     0  0  -1 600;
     0  0  0  1;];
 
robot.MoveL(bTt);
 

%% Make a simple robot program

prog = RDK.AddProgram('Demo prog');
ref = robot.Parent();

targetname = sprintf('P1');
target = RDK.AddTarget(targetname,ref,robot);
target.setPose(bT1);
prog.MoveJ(target);

targetname = sprintf('P2');
target = RDK.AddTarget(targetname,ref,robot);
target.setPose(bTt);
prog.MoveJ(target);

prog.RunProgram();
%% test
clc
close
clear
plot3(0,0,0);
TEST  = FWDKIN2(-10.81,70.22,51.59,-122.51,-77.43,0.52);
K=map_to_graph(-10.81,70.22,51.59,-122.51,-77.43,0.52);
vx=K(1,:);
vy=K(2,:);
vz=K(3,:);

plot3(vx,vy,vz);
drawnow
disp(TEST)


for S = 1:200

    THS=orbitglass([200,15,200],[190,15,200],S/100,S*pi*2/100,50);
    RET=simpleIK(THS);
    VIA=sortvia(RET);

    sz=size(VIA,2);
    
    low1=1000;
    low2=-1;
    for i = 1:sz
        Sums=abs(VIA(2,i),cur(2))+abs(VIA(3,i),cur(3))+abs(VIA(4,i),cur(4))+abs(VIA(5,i),cur(5))+abs(VIA(6,i),cur(6))+abs(VIA(7,i),cur(7));
        if(sums<low1)
            low1=sums;
            low2=i;
        end

    end
    
    cur=VIA(:,low2);
    disp("found");
    K=map_to_graph(cur(2),cur(3),cur(4),cur(5),cur(6),cur(7));
    vx=K(1,:);
    vy=K(2,:);
    vz=K(3,:);

    plot3(vx,vy,vz);
    drawnow

end

%% test
clc
close
clear

%TEST  = FWDKIN2(0,0,0,0,0,0);
TEST  = FWDKIN2(10,20,30,40,50,60);

TESTT  = FWDKIN2(10,48.19,-30,71.8,50,60);
TEST2 = simpleIK(TEST);
VA=sortvia(TEST2);
XT=cullextra(VA,TEST);
disp("output")
disp(XT);
disp("EE matrice")
disp(TEST);
disp(TESTT);
disp(MAPX(TEST,TESTT,0.1))
XC=[0;0;0];
YC=[0;0;0];
ZC=[0;0;0];
PO=[0;0;0];
for D = 1:100
THS=orbitglass([100;15;100],[90;15;100],D,(D*pi*2)/25,60);

XC=[XC,THS(1:3,1)];
YC=[YC,THS(1:3,2)];
ZC=[ZC,THS(1:3,3)];
PO=[PO,THS(1:3,4)];
end

plot3(PO(1,:),PO(2,:),PO(3,:))

hold on
plot3(200,200,200)
for H = 1:101

Sv=PO(:,H);

Xv=(XC(:,H)*10+PO(:,H));
Yv=(YC(:,H)*10+PO(:,H));
Zv=(ZC(:,H)*10+PO(:,H));

plot3([Xv(1),Sv(1)],[Xv(2),Sv(2)],[Xv(3),Sv(3)],"Color",[1,0,0])
plot3([Yv(1),Sv(1)],[Yv(2),Sv(2)],[Yv(3),Sv(3)],"Color",[0,1,0])
plot3([Zv(1),Sv(1)],[Zv(2),Sv(2)],[Zv(3),Sv(3)],"Color",[0,0,1])

end
drawnow


%%
cur=[0,0,0,0,0,0,0];
K=map_to_graph(0,0,0,0,0,0);

for S = 1:50

    THS=orbitglass([200;15;200],[190;15;200],S*2,(S*pi*2)/8,50);
    RET=simpleIK(THS);
    VIA=sortvia(RET);
    sl=size(VIA,1);

    if(sl>5)
    EX=cullextra(VIA,THS);
    sz=size(EX,2);
    sl=size(EX,1);
    low1=1000;
    low2=-1;
    if(sl>5)
    for i = 1:sl
     
        
        Sums=abs(EX(i,2)-cur(2))+abs(EX(i,3)-cur(3))+abs(EX(i,4)-cur(4))+abs(EX(i,5)-cur(5))+abs(EX(i,6)-cur(6))+abs(EX(i,7)-cur(7));
        if(Sums<low1)
            low1=Sums;
            low2=i;
        end

    end
 
    cur=EX(low2,:);
    end
    end
    disp(cur);
    K=map_to_graph(cur(2),cur(3),cur(4),cur(5),cur(6),cur(7));
    vx=K(1,:);
    vy=K(2,:);
    vz=K(3,:);
    
    plot3(vx,vy,vz);
    drawnow
    pause(1);
end



%% kinematic functions
function T=map_to_graph(a1,a2,a3,a4,a5,a6)
zero=[0;0;0;1];
FBT0 = DH( 0    ,   0       ,   173.9           , 0     );
V0=FBT0*zero;
F0T1 = DH( 0        ,   0       ,   0           ,   a1*pi/180        );
V1=FBT0*F0T1*zero;
F1T2 = DH( -pi/2    ,   0       ,   0           ,   a2*pi/180 - pi/2 );
V2=FBT0*F0T1*F1T2*zero;
F2T3 = DH( 0        ,   135     ,   0           ,   a3*pi/180        );
V3=FBT0*F0T1*F1T2*F2T3*zero;
F3T3 = DH( 0        ,   120     ,   0     ,   0 );
V32=FBT0*F0T1*F1T2*F2T3*F3T3*zero;
F3T4 = DH( 0        ,   120     ,   88.78       ,   a4*pi/180 + pi/2 );
V4=FBT0*F0T1*F1T2*F2T3*F3T4*zero;
F4T5 = DH( pi/2     ,   0       ,   95          ,   a5*pi/180        );
V5=FBT0*F0T1*F1T2*F2T3*F3T4*F4T5*zero;
F5T6 = DH( -pi/2    ,   0       ,   0           ,   a6*pi/180        );
V6=FBT0*F0T1*F1T2*F2T3*F3T4*F4T5*F5T6*zero;
F6TE = DH( 0        ,   0       , 65.5          ,   0    );
RET = FBT0 * F0T1 * F1T2 * F2T3 * F3T4 * F4T5 * F5T6 * F6TE;
V7=RET*zero;


VX=[600,600,-600,zero(1), V0(1),V1(1),V2(1),V3(1),V32(1),V4(1),V5(1),V6(1),V7(1)];
VY=[600,600,-600,zero(2), V0(2),V1(2),V2(2),V3(2),V32(2),V4(2),V5(2),V6(2),V7(2)];
VZ=[600,0,0,zero(3), V0(3),V1(3),V2(3),V3(3),V4(3),V32(3),V5(3),V6(3),V7(3)];


T=[VX;VY;VZ];

end

function T=cullextra(test2,FBE)
l=size(test2,1);
RET=0;
first=true;
for s = 1:l
    disp(test2(s,:))
K= FWDKIN2(test2(s,2),test2(s,3),test2(s,4),test2(s,5),test2(s,6),test2(s,7));
if(MAPX(K,FBE,0.2))
if(first)
 RET=test2(s,:);
 first=false;
else
    RET=[RET;test2(s,:)];

end
end

end
T=RET;

end

function T=MAPX(A,B,tol)
RET=true;
if(APX(A(1,1),B(1,1),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(2,1),B(2,1),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(3,1),B(3,1),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(1,2),B(1,2),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(2,2),B(2,2),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(3,2),B(3,2),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(1,3),B(1,3),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(2,3),B(2,3),tol)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(3,3),B(3,3),tol)&&RET)
    RET=true;
else
    RET=false;
end

if(APX(A(1,4),B(1,4),50)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(2,4),B(2,4),50)&&RET)
    RET=true;
else
    RET=false;
end
if(APX(A(3,4),B(3,4),50)&&RET)
    RET=true;
else
    RET=false;
end
T=RET;

end
function T = FramefromPos_XYZ(Xp,Yp,Zp,Xr,Yr,Zr)
    a0=Xr;
    b0=Yr;
    g0=Zr;
    %generate a new rotation matrix
    E1=cos(a0)*sin(b0)*sin(g0)-sin(a0)*cos(g0);
    E2=cos(a0)*sin(b0)*cos(g0)+sin(a0)*sin(g0);
    E3=sin(a0)*sin(b0)*sin(g0)+cos(a0)*cos(g0);
    E4=sin(a0)*sin(b0)*cos(g0)-cos(a0)*sin(g0);

    
T=  [cos(a0)*cos(b0)    E1               E2               Xp;
     sin(a0)*cos(b0)    E3               E4               Yp;
     -sin(b0)           cos(b0)*sin(g0)  cos(b0)*cos(g0)  Zp;
     0                  0                0                1];



end

function T = simpleIK(F)
%disp("test start");
% get solution for v1
VK=F*vector4(0,0,0);
XE=VK(1);
YE=VK(2);
ZE=VK(3);
FBE=F;
FE6=DH(0,0,-65.5,0);
F0B=DH(0,0,-173.9,0);
F06=F0B*FBE*FE6;
X6=F06(1,4);
Y6=F06(2,4);
Z6=F06(3,4);
% test here
%disp("test endpoints");
%disp(FBE*vector4(0,0,0));
%disp(F06*vector4(0,0,0));
% calculate angle
a=atan2(Y6,X6);
xyPlanedist=sqrt(X6*X6+Y6*Y6);
b=asin(88.78/xyPlanedist);
V1_1=a-b;
V1_2=a+b;
V1_3=V1_1+pi;
V1_4=V1_2+pi;
% test here
%disp("test acuracy of v1");
%disp(V1_1*180/pi());

%disp(V1_2*180/pi());



% reform matrix to calculate ee rotation
%rotation1
RK1=ZYZINT(V1_1,F06);
S1S=compute2_from_zyz(V1_1,RK1(1,1),RK1(1,2),RK1(1,3),F06);
S2S=compute2_from_zyz(V1_1,RK1(2,1),RK1(2,2),RK1(2,3),F06);

%rotation2
RK2=ZYZINT(V1_2,F06);
S3S=compute2_from_zyz(V1_2,RK2(1,1),RK2(1,2),RK2(1,3),F06);
S4S=compute2_from_zyz(V1_2,RK2(2,1),RK2(2,2),RK2(2,3),F06);

%rotation3
RK3=ZYZINT(V1_3,F06);
S5S=compute2_from_zyz(V1_3,RK3(1,1),RK3(1,2),RK3(1,3),F06);
S6S=compute2_from_zyz(V1_3,RK3(2,1),RK3(2,2),RK3(2,3),F06);

%rotation2
RK4=ZYZINT(V1_4,F06);
S7S=compute2_from_zyz(V1_4,RK4(1,1),RK4(1,2),RK4(1,3),F06);
S8S=compute2_from_zyz(V1_4,RK4(2,1),RK4(2,2),RK4(2,3),F06);

% return the output
%disp("return");
T=[S1S;S2S;S3S;S4S;S5S;S6S;S7S;S8S];

end

function T = orbitglass(pos1,pos2,centercord,rotation,radius)

dir1= pos2-pos1;
dir1=dir1/norm(dir1);
%dir1 = x1
centerlinecord=dir1*centercord+pos1;

dir11=([1;1;1]-dir1);
dir11=dir11/norm(dir11);

disp(dir1);
disp(dir11);

dir2=cross(dir1,dir11);
dir2=dir2/norm(dir2);
%dir2=z1

dir3=(cross(dir1,dir2));
dir3=-1*dir3/norm(dir3);
%dir3=y1

disp(dir2);
disp(dir3);

distre=dir3*c(rotation)+dir2*sin(rotation)/norm(dir3*c(rotation)+dir2*sin(rotation));
perimiter=centerlinecord+distre*radius;

Z=(centerlinecord-perimiter);
disp("glass point")
Z=Z/norm(Z);
Y=(cross(Z,dir1));
Y=Y/norm(Y)*-1;
X=(cross(Z,Y));
X=X/norm(X)*-1;
disp(X)
disp(Y)
disp(Z)
T=[X,Y,Z,perimiter;0,0,0,1];
disp(T);

end
