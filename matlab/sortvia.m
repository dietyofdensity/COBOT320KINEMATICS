function VIA = sortvia(TEST2)
VIABLES=0;
first=true;
for R = 1:16
    RET=true;
    if(TEST2(R,1)<1)
       
       for K = 1:6
           A=0;
           B=true;
           a=TEST2(R,K+1);
            if(K==6)
                A = setinrange(a,180);
                B = APX(A,0,175+1);
            else
                A=setinrange(a,180);
                B= APX(A,0,165+1);
            end
            
            TEST2(R,K+1)=A;
        if(B==false)
            RET=false;

            

        end
             

       end
       if(RET)
        if(first)
           VIABLES=TEST2(R,1:7);
           first=false;

        else
           VIABLES=[VIABLES;TEST2(R,1:7)];
       
        end
       end
        
    end

end

VIA=VIABLES;
end