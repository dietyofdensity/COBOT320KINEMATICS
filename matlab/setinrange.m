
function T= setinrange(a,period)


if(a<-period)
a=a+2*period;
elseif(a>period)
    a=a-2*period;
end

if(a<-period)
    a=a+2*period;

elseif(a>period)
    a=a-2*period;

end

T=a;

end

