function T = FWDKIN2(a1,a2,a3,a4,a5,a6)
FBT0 = DH( 0    ,   0       ,   173.9           , 0     );
F0T1 = DH( 0        ,   0       ,   0           ,   a1*pi/180        );
F1T2 = DH( -pi/2    ,   0       ,   0           ,   a2*pi/180 - pi/2 );
F2T3 = DH( 0        ,   135     ,   0           ,   a3*pi/180        );
F3T4 = DH( 0        ,   120     ,   88.78       ,   a4*pi/180 + pi/2 );
F4T5 = DH( pi/2     ,   0       ,   95          ,   a5*pi/180        );
F5T6 = DH( -pi/2    ,   0       ,   0           ,   a6*pi/180        );
F6TE = DH( 0        ,   0       , 65.5          ,   0    );
RET = FBT0 * F0T1 * F1T2 * F2T3 * F3T4 * F4T5 * F5T6 * F6TE;

T=RET;
end