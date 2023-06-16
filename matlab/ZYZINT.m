function T = ZYZINT(V1,F06)
%UNTITLED13 Summary of this function goes here
%   Detailed explanation goes here

R01=DH(0,0,0,V1);
R13=DH(-pi() / 2, 0, 0, -pi() / 2);
R03=R01*R13;
R36=inv(R03)*F06;

R13=R36(1,3);
R23=R36(2,3);
R31=R36(3,1);
R32=R36(3,2);
R33=R36(3,3);
R11=R36(1,1);
R12=R36(1,2);

beta_1=atan2(sqrt(R31*R31+R32*R32),R33);
alpha_1=atan2(R23/sin(beta_1),R13/sin(beta_1));
gamma_1=atan2(R32/sin(beta_1),-R31/sin(beta_1));


beta_2=atan2(-sqrt(R31*R31+R32*R32),R33);
alpha_2=atan2(R23/sin(beta_2),R13/sin(beta_2));
gamma_2=atan2(R32/sin(beta_2),-R31/sin(beta_2));


if(APX(beta_1,pi,0.01))
     % insert code to find shortest distance to base
    alpha_1=0;
    gamma_1=atan2(-R12,R11);

end
if(APX(beta_1,0,0.01))
    
    alpha_1=0;
    gamma_1=atan2(R12,-R11);

   
end
if(APX(beta_2,pi,0.01))
     % insert code to find shortest distance to base
   

    alpha_2=0;
    gamma_2=atan2(-R12,R11);
end
if(APX(beta_2,0,0.01))
    
  

    alpha_2=0;
    gamma_2=atan2(R12,-R11);
end
ang1 = [alpha_1,beta_1,gamma_1];
ang2 = [alpha_2,beta_2,gamma_2];
T=[ang1;ang2];

end