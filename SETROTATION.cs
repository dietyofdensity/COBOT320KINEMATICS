using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SETROTATION : MonoBehaviour
{
    public Text UITEXT;

    float sum = 0;
    // set up input variables and robot model refrences

    public GameObject joint1g;
    Quaternion q01;
    public GameObject joint2g;
    Quaternion q02;
    public GameObject joint3g;
    Quaternion q03;
    public GameObject joint4g;
    Quaternion q04;
    public GameObject joint5g;
    Quaternion q05;
    public GameObject joint6g;
    Quaternion q06;

    public float joint1;
    public float joint2;
    public float joint3;
    public float joint4;
    public float joint5;
    public float joint6;

    public bool update = false;

    public Vector3 rot;
    public Vector3 pos;

    public GameObject toolframe;
    int selected = 1000;
    List<float[]> soluton;

    int cnt = 0;
    public Vector3 cuppos;
    public Vector3 cuppos2;
    // Start is called before the first frame update
    void Start()
    {
        soluton = new List<float[]>();
        //save the zero rotation of each joint
        q01 = joint1g.transform.rotation;
        q02 = joint2g.transform.rotation;
        q03 = joint3g.transform.rotation;
        q04 = joint4g.transform.rotation;
        q05 = joint5g.transform.rotation;
        q06 = joint6g.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //if one of the input joints changed update all rotatio ns
        if (sum != (joint1 + joint2 + joint3 + joint4 + joint5 + joint6))
        {
            Debug.Log("moved");
            joint1g.transform.rotation = q01;
            joint2g.transform.rotation = q02;
            joint3g.transform.rotation = q03;
            joint4g.transform.rotation = q04;
            joint5g.transform.rotation = q05;
            joint6g.transform.rotation = q06;
            joint1g.transform.Rotate(-Vector3.up * joint1);
            joint2g.transform.Rotate(Vector3.forward * joint2);
            joint3g.transform.Rotate(Vector3.forward * joint3);
            joint4g.transform.Rotate(Vector3.forward * joint4);
            joint5g.transform.Rotate(-Vector3.up * joint5);
            joint6g.transform.Rotate(Vector3.forward * joint6);


        }

        //if the update button is pressed
        if (update)
        {
            cnt++;
            Debug.Log(setinlimit(420, 165, -165, 180));
            //run the test code
            debugfunk();
            Debug.Log(setendframe().inverse);

            update = false;
        }


        sum = joint1 + joint2 + joint3 + joint4 + joint5 + joint6;
    }
    /*
    private void FixedUpdate()
    {
        StartCoroutine(tick());
    }
    
    private IEnumerator tick()
    {
        yield return new WaitForSeconds(5f);
        cnt++;
        Debug.Log(setinlimit(420, 165, -165, 180));
        //run the test code
        debugfunk();
        Debug.Log(setendframe().inverse);

        update = false;
    }
    */
    public float[] xyzRPY(Matrix4x4 inp)
    {
       //float yaw = Mathf.Atan2(inp(2, 1), inp(1, 1));
       // float pitch = Mathf.Atan2(-inp(3, 1), sqrt(inp(3, 2) ^ 2 + inp(3, 3) ^ 2)));
       // float roll = Mathf.Atan2(inp(3, 2), inp(3, 3));
        float yaw = Mathf.Atan2(inp.m10, inp.m00);
        float pitch = Mathf.Atan2(-inp.m20, Mathf.Sqrt(inp.m21* inp.m21 + inp.m22* inp.m22));
        float roll = Mathf.Atan2(inp.m21, inp.m22);


        return new float[] { inp.m30, inp.m31, inp.m32,roll,pitch,yaw };
    }

    (float a, bool b) setinlimit(float a, float max, float min, float period)
    {

        if (a > period)
        {
            a = a - period * 2;
        }
        else if (a < -period)
        {
            a = a + period * 2;
        }

        if (a > period)
        {
            a = a - period * 2;
        }
        else if (a < -period)
        {
            a = a + period * 2;
        }

        if (a > period)
        {
            a = a - period * 2;
        }
        else if (a < -period)
        {
            a = a + period * 2;
        }


        return (a, isinrange(a, max, min));




    }
    bool isinrange(float a, float max, float min)
    {
        return (a < max && a > min);
    }
    Matrix4x4 setendframe()
    {
        Vector3 rotation = rot;
        Vector3 position = pos;
        float a = rotation.x * Mathf.Deg2Rad;
        float b = rotation.y * Mathf.Deg2Rad;
        float g = rotation.z * Mathf.Deg2Rad;

        Vector4 col1 = new Vector4(
           c(a) * c(b),
           s(a) * c(b),
           -s(b),
           0);
        Vector4 col2 = new Vector4(
            c(a) * s(b) * s(g) - s(a) * c(g),
            s(a) * s(b) * s(g) + c(a) * c(g),
            c(b) * s(g),
            0);
        Vector4 col3 = new Vector4(
            c(a) * s(b) * c(g) + s(a) * s(g),
            s(a) * s(b) * c(g) - c(a) * s(g),
            c(b) * c(g),
            0);
        Vector4 col4 = new Vector4(
            position.x,
            position.y,
            position.z,
            1);
        Matrix4x4 ret = new Matrix4x4(col1, col2, col3, col4);

        return ret;


    }

    Matrix4x4 orbitglass(float step,float xline,float radius)
    {
        

        Vector3 direction = (cuppos2 - cuppos).normalized;
        Vector3 centerlinecord = direction.normalized * xline + cuppos;

        Debug.DrawLine(flip(centerlinecord), flip(centerlinecord + direction * 20), Color.red, float.MaxValue);
        Matrix4x4 baseline =Matrix4x4.Translate(cuppos);
        Vector3 dir2 = (Vector3.one - direction).normalized;
        
        Vector3 dir3 = Vector3.Cross(direction, dir2).normalized;
        Debug.DrawLine(flip(centerlinecord), flip(centerlinecord + dir3 * 20), Color.blue, float.MaxValue);

        Vector3 dir4=Vector3.Cross(direction, dir3).normalized;
        Debug.DrawLine(flip(centerlinecord), flip(centerlinecord + dir4 * 20), Color.green, float.MaxValue);
        Vector3 perimiter=centerlinecord +(dir4*c(step)+dir3*s(step))*radius;

        Debug.DrawLine(flip(centerlinecord), flip(perimiter), Color.green, float.MaxValue);


        Vector3 z = (centerlinecord - perimiter).normalized;
        Vector3 x = direction.normalized;
        Vector3 y = Vector3.Cross (x, z).normalized*-1;
        Debug.DrawLine(flip(perimiter), flip(perimiter + x * 20), Color.red, float.MaxValue);
        Debug.DrawLine(flip(perimiter), flip(perimiter + z * 20), Color.blue, float.MaxValue);
        Debug.DrawLine(flip(perimiter), flip(perimiter + y * 20), Color.green, float.MaxValue);
        Vector4 col1 = x;
        col1.w = 0;
        Vector4 col2 = y;
        col2.w = 0;
        Vector4 col3 = z;
        col3.w = 0;
        Vector4 col4 = perimiter;
        Debug.Log("distance to point was "+perimiter.magnitude);
        col4.w = 1;
        //Matrix4x4 k = new Matrix4x4(col1,col2,col3,col4);
        Matrix4x4 k = new Matrix4x4(col1, col2, col3, col4);
        return k;
    }

    void debugfunk()
    {
        float[] cur = {joint1*Mathf.Deg2Rad,joint2 * Mathf.Deg2Rad, joint3 * Mathf.Deg2Rad, joint4 * Mathf.Deg2Rad, joint5 * Mathf.Deg2Rad, joint6 * Mathf.Deg2Rad };
        float rtd = Mathf.Rad2Deg;
        Vector4 pos = new Vector4(0, 0, 0, 1);
        // get the desired end effector matrix based on joint rotations
        Matrix4x4 fk = Fwdkin(joint1, joint2, joint3, joint4, joint5, joint6);
        float[] XYZRP = xyzRPY(fk);
        Debug.Log("XYZRPY  X "+ XYZRP[0]+" Y " + XYZRP[1] + "Z " + XYZRP[2] + "R " + XYZRP[3]*rtd + "P " + XYZRP[4]*rtd + "Y " + XYZRP[5]*rtd + " ");

        //start building the output string
        string textinput = $"input angles were \n v1: {joint1},v2: {joint2},v3: {joint3},v4: {joint4},v5: {joint5},v6: {joint6} \n";
        //run the inverse kinematics
        int tcount = cnt ;
        float trx = ((float)tcount) / 50;
        float rex = ((float)tcount) * pi() * 2 / 100f;

        Matrix4x4 Dframe = orbitglass(rex, 40+tcount, 100);
        Debug.Log("desired frame was"+Dframe);
        // List<float[]> ik = ikin(Dframe);
         List<float[]> ik = ikin(fk);
        // List<float[]> ik = ikin(fk);
        Debug.Log("forward kin was" + fk);
        Matrix4x4 sr = setendframe();
        Debug.Log("set frame was" + sr);
        Debug.Log(fk * pos);

        textinput += "found solutions \n";
      
        for (int i = 0; i < ik.Count; i++)
        {
            float[] a = ik[i];
            for (int x = 1; x < 7; x++)
            {
                float ang = a[x];
                float dtr = Mathf.Deg2Rad;
                
                (float a, bool b) angle = setinlimit(ang, 165 * dtr, -165 * dtr, pi());
                if (angle.b)
                {
                    ik[i][x] = angle.a;
                }
                else if (x==6)
                {
                    angle = setinlimit(ang, 175 * dtr, -175 * dtr, pi());
                    if (angle.b)
                    {
                        ik[i][x] = angle.a;
                    }
                    else
                    {
                        ik[i][0] += 255;
                    }

                }
                else
                {
                    ik[i][0] += 255;
                }

                
            }
        }
        soluton.Clear();
        List<int> sol = new List<int>();
        for (int i = 0; i < ik.Count; i++)
        {
            //if the distance between joint 1 and joint 4 is less than 3, to see if it is in range
            if (ik[i][0] < 3)
            {
                //with the found rotations generate a matrix wit forward kinematcs
                Matrix4x4 thissol = Fwdkin(ik[i][1] * rtd, ik[i][2] * rtd, ik[i][3] * rtd, ik[i][4] * rtd, ik[i][5] * rtd, ik[i][6] * rtd);
                //if this rotation yelds a matrix similar to the original frame save it
                if (matrixaprox(fk, thissol, 0.1f))
                {
                    soluton.Add(ik[i]);
                    Debug.Log($"V1: {ik[i][1] * rtd} ; V2: {ik[i][2] * rtd}; V3: {ik[i][3] * rtd}; V4: { ik[i][4] * rtd}; V5: {ik[i][5] * rtd}; V6: {ik[i][6] * rtd}");
                    textinput += $"<{i}>-< V1: {ik[i][1] * rtd} ; V2: {ik[i][2] * rtd}; V3: {ik[i][3] * rtd}; V4: { ik[i][4] * rtd}; V5: {ik[i][5] * rtd}; V6: {ik[i][6] * rtd} > \n";
                    sol.Add(i);
                }
            }
        }

        
        float lowest=1000;
        int loweeest=-1;
        for (int i = 0; i < sol.Count; i++)
        {
            float[] f = ik[sol[i]];
            float sum =  Mathf.Abs(f[1]-cur[0] ) + Mathf.Abs(f[2] - cur[1]) + Mathf.Abs(f[3] - cur[2]) + Mathf.Abs(f[4] - cur[3]) + Mathf.Abs(f[5] - cur[4]) + Mathf.Abs(f[6] - cur[5]);
            if(sum < lowest)
            {
                lowest = sum;
                loweeest = sol[i];
            }
            
        }
        if(loweeest > -1)
        {
            float[] fo = ik[loweeest];
        joint1 = fo[1] * rtd;
        joint2 = fo[2] * rtd;
        joint3 = fo[3] * rtd;
        joint4 = fo[4] * rtd;
        joint5 = fo[5] * rtd;
        joint6 = fo[6] * rtd;

        }
       


    
        UITEXT.text = textinput;
    }

    public void up()
    {
        selected--;
        setjoints(soluton[(selected % soluton.Count)]);
    }
    public void down()
    {
        selected++;
        setjoints(soluton[(selected % soluton.Count)]);

    }
    public void setjoints(float[] f)
    {

        Debug.Log($"selected {selected} of {soluton.Count} solutions");

        float rtd = Mathf.Rad2Deg;

        joint1 = f[1] * rtd;
        joint2 = f[2] * rtd;
        joint3 = f[3] * rtd;
        joint4 = f[4] * rtd;
        joint5 = f[5] * rtd;
        joint6 = f[6] * rtd;

    }

    void debugview(float[] f, int i)
    {
        Vector3 ofset = Vector3.forward * 500 * i;
        Debug.DrawLine(Vector3.zero, ofset, Color.black, float.MaxValue);
        Vector3 org = new Vector3(0, 0, 0);
        Vector3 point = new Vector3(0, 0, 100);
        if (f[0] < 0.01)
        {
            float rtd = Mathf.Rad2Deg;
            Matrix4x4 ep = Fwdkin(f[1] * rtd, f[2] * rtd, f[3] * rtd, f[4] * rtd, f[5] * rtd, f[6] * rtd);
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + ep.MultiplyPoint3x4(point), Color.blue, float.MaxValue);

            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + ep.MultiplyPoint3x4(point), Color.blue, float.MaxValue);

            Matrix4x4 F6E = DHPFRAME(0, 0, 65.5f, 0);
            point = ep.MultiplyPoint3x4(org);
            ep = ep * F6E.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);
            Matrix4x4 F56 = DHPFRAME(-pi() / 2, 0, 0, f[6]);

            point = ep.MultiplyPoint3x4(org);
            ep = ep * F56.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);
            Matrix4x4 F45 = DHPFRAME(pi() / 2, 0, 95, f[5]);

            point = ep.MultiplyPoint3x4(org);
            ep = ep * F45.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);
            Matrix4x4 F34 = DHPFRAME(0, 120, 88.78f, f[4] + pi() / 2);

            point = ep.MultiplyPoint3x4(org);
            ep = ep * F34.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);
            Matrix4x4 F23 = DHPFRAME(0, 135, 0, f[3]);

            point = ep.MultiplyPoint3x4(org);
            ep = ep * F23.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);
            Matrix4x4 F12 = DHPFRAME(-pi() / 2, 0, 0, f[2] - pi() / 2);

            point = ep.MultiplyPoint3x4(org);
            ep = ep * F12.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);
            Matrix4x4 F01 = DHPFRAME(0, 0, 0, f[1]);

            point = ep.MultiplyPoint3x4(org);
            ep = ep * F01.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);
            Matrix4x4 FB0 = DHPFRAME(0, 0, 173.9f, 0);

            point = ep.MultiplyPoint3x4(org);
            ep = ep * FB0.inverse;
            Debug.DrawLine(ofset + ep.MultiplyPoint3x4(org), ofset + point, Color.blue, float.MaxValue);







        }





    }
    float s(float x)
    {
        return Mathf.Sin(x);

    }
    float sa(float x)
    {
        return Mathf.Asin(x);

    }
    float c(float x)
    {
        return Mathf.Cos(x);

    }
    float ca(float x)
    {
        return Mathf.Acos(x);

    }
    float pi()
    {
        return Mathf.PI;

    }
    float CoRC(float a, float b, float c)
    {

        return Mathf.Acos((a * a + b * b - c * c) / (2 * a * b));
    }
    float CoRB(float a, float b, float c)
    {


        return Mathf.Acos((a * a + c * c - b * b) / (2 * a * c));
    }
    float CoRA(float a, float b, float c)
    {


        return Mathf.Acos((c * c + b * b - a * a) / (2 * c * b));
    }

    Matrix4x4 getdesiredposition()
    {
        Matrix4x4 a = new Matrix4x4();




        return a;
    }
    Matrix4x4 Fwdkin(float a1, float a2, float a3, float a4, float a5, float a6)
    {
        float dtr = Mathf.Deg2Rad;
        Matrix4x4 FB0 = DHPFRAME(0, 0, 173.9f, 0);
        Matrix4x4 F01 = DHPFRAME(0, 0, 0, a1 * dtr);
        Matrix4x4 F12 = DHPFRAME(-pi() / 2, 0, 0, a2 * dtr - pi() / 2);
        Matrix4x4 F23 = DHPFRAME(0, 135, 0, a3 * dtr);
        Matrix4x4 F34 = DHPFRAME(0, 120, 88.78f, a4 * dtr + pi() / 2);
        Matrix4x4 F45 = DHPFRAME(pi() / 2, 0, 95, a5 * dtr);
        Matrix4x4 F56 = DHPFRAME(-pi() / 2, 0, 0, a6 * dtr);
        Matrix4x4 F6E = DHPFRAME(0, 0, 65.5f, 0);
        Matrix4x4 ret = FB0 * F01 * F12 * F23 * F34 * F45 * F56 * F6E;

        return ret;
    }

    Matrix4x4 DHPFRAME(float alpha, float a, float d, float theta)
    {
        Vector4 col1 = new Vector4(
            c(theta),
            s(theta) * c(alpha),
            s(theta) * s(alpha),
            0);
        Vector4 col2 = new Vector4(
            -s(theta),
            c(theta) * c(alpha),
            c(theta) * s(alpha),
            0);
        Vector4 col3 = new Vector4(
            0,
            -s(alpha),
            c(alpha),
            0);
        Vector4 col4 = new Vector4(
            a,
            -s(alpha) * d,
            c(alpha) * d,
            1);
        Matrix4x4 ret = new Matrix4x4(col1, col2, col3, col4);

        return ret;
    }

    List<float[]> ikin(Matrix4x4 F_BE)
    {
        List<float[]> list = new List<float[]>();
        Vector4 zero4 = new Vector4(0, 0, 0, 1);
        //use dh matrixes to get a F06 frame based on the end effector rotation
        Matrix4x4 F_0B = DHPFRAME(0, 0, -173.9f, 0);
        Matrix4x4 F_E6 = DHPFRAME(0, 0, -65.5f, 0);
        Matrix4x4 F_06 = F_0B * F_BE * F_E6;

        //get the position of joint 6
        Vector4 P6 = F_06 * zero4;
        Debug.Log(P6);
        // turn vector for debug purposes
        Vector3 dis6 = P6;
        dis6.z = dis6.y;
        dis6.y = 0;
        Debug.DrawLine(Vector3.zero, dis6, Color.red, float.MaxValue);
        Debug.DrawLine(Vector3.zero, Vector3.right * 200, Color.yellow, float.MaxValue);
        //calculate the angel atan angel of the arm in the XY plane
        float aa = Mathf.Atan2(P6.y, P6.x);
        //calculate the ofset between the aa angel and the actual v1 angel
        float bb = Mathf.Asin(88.78f / dis6.magnitude);

        //find all possible solutions for V1 for  this specific end efector position
        float V1_1 = aa - bb; //if 
        float V1_2 = aa + bb;


        float V1_3 = V1_1 + pi();
        float V1_4 = pi() + V1_2;

        #region debugv1

        Vector3 sol1 = rotv(Vector3.right * dis6.magnitude, V1_1);
        Vector3 sol2 = rotv(Vector3.right * dis6.magnitude, V1_2);

        Vector3 sol3 = rotv(Vector3.right * dis6.magnitude, V1_3);
        Vector3 sol4 = rotv(Vector3.right * dis6.magnitude, V1_4);

        Debug.DrawLine(Vector3.zero, sol1, Color.green, float.MaxValue);
        Debug.DrawLine(Vector3.zero, sol2, Color.green, float.MaxValue);
        Debug.DrawLine(Vector3.zero, sol3, Color.green, float.MaxValue);
        Debug.DrawLine(Vector3.zero, sol4, Color.green, float.MaxValue);
        // 


        #endregion
        #region calcalsol
        //use this angel to isolate an end effector rotation
        (Vector3 a, Vector3 b) RO_1 = zyzint(F_06, V1_1);
        //this yelds two solutions in most cases
        //then calculate 2 solutions based on the distance between joint 1 and 4 with these end effector rotations
        (float[] a, float[] b) L1 = calc2fromrot(RO_1.a, F_06, V1_1);
        (float[] a, float[] b) L2 = calc2fromrot(RO_1.b, F_06, V1_1);


        //repeat for all other possible V1s
        (Vector3 a, Vector3 b) RO_2 = zyzint(F_06, V1_2);
        (float[] a, float[] b) L3 = calc2fromrot(RO_2.a, F_06, V1_2);
        (float[] a, float[] b) L4 = calc2fromrot(RO_2.b, F_06, V1_2);
        (Vector3 a, Vector3 b) RO_3 = zyzint(F_06, V1_3);
        (float[] a, float[] b) L5 = calc2fromrot(RO_3.a, F_06, V1_3);
        (float[] a, float[] b) L6 = calc2fromrot(RO_3.b, F_06, V1_3);
        (Vector3 a, Vector3 b) RO_4 = zyzint(F_06, V1_4);
        (float[] a, float[] b) L7 = calc2fromrot(RO_4.a, F_06, V1_4);
        (float[] a, float[] b) L8 = calc2fromrot(RO_4.b, F_06, V1_4);
        //add these values to a list which wil be sorted troug later
        list.Add(L1.a);
        list.Add(L1.b);
        list.Add(L2.a);
        list.Add(L2.b);
        list.Add(L3.a);
        list.Add(L3.b);
        list.Add(L4.a);
        list.Add(L4.b);
        list.Add(L5.a);
        list.Add(L5.b);
        list.Add(L6.a);
        list.Add(L6.b);
        list.Add(L7.a);
        list.Add(L7.b);
        list.Add(L8.a);
        list.Add(L8.b);
        #endregion
        #region remove unusable solutions

        #endregion

        return list;
    }
    Vector3 incaseofzero(Vector3 a, Matrix4x4 F_06)
    {

        Vector3 b = a * Mathf.Rad2Deg;

        float cang = b.x + b.z;

        int shortid = 0;
        float shortes = 10000;

        for (int i = 0; i < 360; i++)
        {
            Vector3 c = new Vector3(i, b.y, cang - i);
            float dist = getdist(c * Mathf.Deg2Rad, F_06);
            if (dist < shortes)
            {
                shortes = dist;
                shortid = i;

            }


        }
        b = new Vector3(shortid, b.y, cang - shortid);
        b = b * Mathf.Deg2Rad;
        Debug.Log($"continuing with vector {b * Mathf.Rad2Deg}");



        return b;
    }

    float getdist(Vector3 a, Matrix4x4 F_06)
    {
        Matrix4x4 F4P4 = DHPFRAME(0, 0, 88.78f, a.x + pi() / 2);
        Matrix4x4 F45 = DHPFRAME(pi() / 2, 0, 95, -a.y);
        Matrix4x4 F56 = DHPFRAME(-pi() / 2, 0, 0, a.z);
        Matrix4x4 F4P6 = F4P4 * F45 * F56;
        Matrix4x4 F04P = F_06 * F4P6.inverse;
        Vector3 P4 = F04P * new Vector4(0, 0, 0, 1);

        return P4.magnitude;
    }
    Vector3 rotv(Vector3 v, float a)
    {
        return Quaternion.AngleAxis(a * Mathf.Rad2Deg, Vector3.down) * v;
    }
    (Vector3 a, Vector3 b) zyzint(Matrix4x4 F06, float v1)
    {
        //first we simplify the F06 frame using the possible v1 angle
        Matrix4x4 F01 = DHPFRAME(0, 0, 0, v1);
        Matrix4x4 F13 = DHPFRAME(-pi() / 2, 0, 0, -pi() / 2);

        Matrix4x4 F03 = F01 * F13;
        //we then use this to calculate the rotation matrix needed to be mimiked by the last 3 joints
        //this is done using a ZYZ interpetation of the matrix.
        Matrix4x4 F36 = F03.inverse * F06;
        Vector3 a = new Vector3();
        Vector3 b = new Vector3();
        //x=alpha
        //y=beta
        //z=gamma
        //we 
        float R31 = F36[2, 0];
        float R32 = F36[2, 1];
        float R33 = F36[2, 2];
        float R13 = F36[0, 2];
        float R23 = F36[1, 2];

        a.y = Mathf.Atan2(Mathf.Sqrt(R31 * R31 + R32 * R32), R33);
        a.x = Mathf.Atan2(R23 / s(a.y), R13 / s(a.y));
        a.z = Mathf.Atan2(R32 / s(a.y), -R31 / s(a.y));

        b.y = Mathf.Atan2(-Mathf.Sqrt(R31 * R31 + R32 * R32), R33);
        b.x = Mathf.Atan2(R23 / s(b.y), R13 / s(b.y));
        b.z = Mathf.Atan2(R32 / s(b.y), -R31 / s(b.y));

        float tol = 0.1f;
        bool is0 = (a.y * Mathf.Rad2Deg < tol && a.y * Mathf.Rad2Deg > -tol);
        bool ispi = (Mathf.Approximately(a.y, pi()));

        if (a.y * Mathf.Rad2Deg < 0.1 && a.y * Mathf.Rad2Deg > -0.1)
        {
            Debug.Log("angle 5 was " + a.y * Mathf.Rad2Deg + " so it is aproximatly 0 ?:" + Mathf.Approximately(a.y, 0));

        }

        if (ispi)
        {
            float R12 = F36[0, 1];
            float R11 = F36[0, 0];
            float zrot = Mathf.Atan2(R12, -R11);
            a.x = zrot;
            b.x = 0;
            Debug.Log("v5 was aproximatly pi: " + v1 * Mathf.Rad2Deg + "zrot was :" + zrot * Mathf.Rad2Deg);
            a.z = 0;
            b.z = zrot;
            a = incaseofzero(b, F06);
            b = a;
        }
        if (is0)
        {
            float R12 = F36[0, 1];
            float R11 = F36[0, 0];
            float zrot = Mathf.Atan2(-R12, R11);
            a.x = zrot;
            b.x = 0;
            Debug.Log("v5 was aproximatly 0: " + v1 * Mathf.Rad2Deg + "zrot was :" + zrot * Mathf.Rad2Deg);
            a.z = 0;
            b.z = zrot;
            a = incaseofzero(b, F06);
            b = a;
        }

        return (a, b);


    }
    Vector3 flip(Vector3 v)
    {
        float x = v.x;
        float y = v.y;  
        float z = v.z;
        v.x = x;
        v.z = y;
        v.y = z;
        return v;

    }
    bool matrixaprox(Matrix4x4 a, Matrix4x4 b, float tol)
    {
        bool ret = true;
        //chek rotation
        ret = fapx(a[0, 0], b[0, 0], tol) ? ret : false;
        ret = fapx(a[1, 0], b[1, 0], tol) ? ret : false;
        ret = fapx(a[2, 0], b[2, 0], tol) ? ret : false;
        ret = fapx(a[0, 1], b[0, 1], tol) ? ret : false;
        ret = fapx(a[1, 1], b[1, 1], tol) ? ret : false;
        ret = fapx(a[2, 1], b[2, 1], tol) ? ret : false;
        ret = fapx(a[0, 2], b[0, 2], tol) ? ret : false;
        ret = fapx(a[1, 2], b[1, 2], tol) ? ret : false;
        ret = fapx(a[2, 2], b[2, 2], tol) ? ret : false;
        //chek position
        ret = fapx(a[0, 3], b[0, 3], 50) ? ret : false;
        ret = fapx(a[1, 3], b[1, 3], 50) ? ret : false;
        ret = fapx(a[2, 3], b[2, 3], 50) ? ret : false;

        return ret;


    }
    bool fapx(float a, float b, float tol)
    {
        return (a - b > -tol && a - b < tol);
    }

    (float[] a, float[] b) calc2fromrot(Vector3 r, Matrix4x4 F_06, float v1)
    {
        float[] a;
        float[] b;
        Matrix4x4 F4P4 = DHPFRAME(0, 0, 88.78f, r.x + pi() / 2);
        Matrix4x4 F45 = DHPFRAME(pi() / 2, 0, 95, -r.y);
        Matrix4x4 F56 = DHPFRAME(-pi() / 2, 0, 0, r.z);
        Matrix4x4 F4P6 = F4P4 * F45 * F56;
        Matrix4x4 F04P = F_06 * F4P6.inverse;
        Vector3 P4 = F04P * new Vector4(0, 0, 0, 1);
        Vector3 P4xy = new Vector3(P4.x, P4.y, 0);

        float distxyz = P4.magnitude;
        float distxy = P4xy.magnitude;
        float L2 = 135.002f;
        float L3 = 120.002f;
        float direction = Vector3.Dot(rotv(Vector3.right, v1), P4xy / distxy);
        float dir = (direction > 0) ? -1 : 1;

        float alpha = Mathf.Atan2(P4.z, distxy * dir) - pi() / 2;
        // Debug.Log($"distxy was {distxy } Z was {P4.z} alpha was {alpha * Mathf.Rad2Deg} dir was {dir} ");
        float A = CoRA(L2, L3, distxyz);
        float C = CoRC(L2, L3, distxyz);
        float B = CoRB(L2, L3, distxyz);
        //  Debug.Log($"distxyz was {distxyz } A was {A * Mathf.Rad2Deg } C was {C * Mathf.Rad2Deg} B was {B * Mathf.Rad2Deg}");

        float V2_1;
        float V2_2;

        float V3_1;
        float V3_2;

        float V4_1;
        float V4_2;


        V2_1 = alpha - B;
        V2_2 = alpha + B;

        V3_1 = pi() - C;
        V3_2 = -pi() + C;

        V4_1 = r.x - (pi() / 2 + alpha + A);
        V4_2 = r.x - (pi() / 2 + alpha - A);





        float v5_1 = -r.y;
        float v5_2 = -r.y;

        float v6_1 = r.z;
        float v6_2 = r.z;

        a = new float[] { distxyz - (L2 + L3), v1, V2_1, V3_1, V4_1, v5_1, v6_1 };
        b = new float[] { distxyz - (L2 + L3), v1, V2_2, V3_2, V4_2, v5_2, v6_2 };


        return (a, b);
    }

    private static Texture2D GetTextureFromCamera(Camera mCamera)
    {
        Rect rect = new Rect(0, 0, mCamera.pixelWidth, mCamera.pixelHeight);
        RenderTexture renderTexture = new RenderTexture(mCamera.pixelWidth, mCamera.pixelHeight, 24);
        Texture2D screenShot = new Texture2D(mCamera.pixelWidth, mCamera.pixelHeight, TextureFormat.RGBA32, false);

        mCamera.targetTexture = renderTexture;
        mCamera.Render();

        RenderTexture.active = renderTexture;

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();


        mCamera.targetTexture = null;
        RenderTexture.active = null;
        return screenShot;
    }
}
