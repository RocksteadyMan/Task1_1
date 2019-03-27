using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    public WheelCollider[] WCL;//левые колёса
    public float[] speedCL;//скорость движения вперёд(назад) левых колёс
    public float[] speedRotatingCL;//скорость поворота колёс(Вправо, влево) левых колёс
    public WheelCollider[] WCR;//правые колёса
    public float[] speedCR;//скорость движения вперёд(назад) правых колёс
    public float[] speedRotatingCR;//скорость поворота колёс(Вправо, влево) правых колёс
    public float maxBrakeForce;//максимальная сила торможения колёс
    public float brakeForce;//сила тороможения колёс
    float vertical, horizontal; // направление езды машины
    float space;
    //текущая скорость колеса
    public float currentSpeedWCLW;
    public float currentSpeedWCRW;
    public float currentSpeedWCLB;
    public float currentSpeedWCRB;
    
    public float currentMiddleSpeed;//текущая средняя скорость всей машины
    public float maxSpeed = 60;//максимальная скорость 
    //для спидометра
    public float minSpeedAngle;
    public float maxSpeedAngle;
    public float currentSpeedAngle;
    public float steepSpeedAngle;
  
    public Transform arrow;//стрелка спидометра


    private void Start()
    {
        //расчёт шага изменения угла стрелки спидометра
        steepSpeedAngle = 270 / maxSpeed;
    }
    //вызывается в конце каждого кадра
    private void Update()
    {
        //управление: vertical - вперёд, назад ехать, horizontal - вправо, влево, jump - пробел, т.е. ручник
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        space = Input.GetAxis("Jump");
        //вычисление текущей скорости машины, взял в интернете
        currentSpeedWCLW = 2 * Mathf.PI * WCL[0].radius * WCL[0].rpm * 1000;
        currentSpeedWCLB = 2 * Mathf.PI * WCL[1].radius * WCL[1].rpm * 1000;

        currentSpeedWCRW = 2 * Mathf.PI * WCR[0].radius * WCR[0].rpm * 1000;
        currentSpeedWCRB = 2 * Mathf.PI * WCR[1].radius * WCR[1].rpm * 1000;
        //средняя скорость всей машины
        currentMiddleSpeed = (currentSpeedWCLB + currentSpeedWCLW + currentSpeedWCRB + currentSpeedWCRW) / 4;
        //поворот стрелки в зависимости от текущей средней скорости
        arrow.localEulerAngles = new Vector3(0, 0, minSpeedAngle);
        arrow.rotation *= Quaternion.Euler(0, 0, steepSpeedAngle * Mathf.Abs( currentMiddleSpeed));
    }
    
    void FixedUpdate()
    {
        UpdateWheels(vertical,horizontal, space);
    }

    void UpdateWheels(float vertical, float horizontal, float space)
    {
        //проходим по массивам левых колёс
        for (int i = 0; i < 2; i++)
        {
            MoveNRotate(WCL[i], vertical, horizontal, space, speedCL[i], speedRotatingCL[i]);
        }
        
        for (int i = 0; i < 2; i++)
        {
            MoveNRotate(WCR[i], vertical, -horizontal, space, speedCR[i], speedRotatingCR[i]);
        }
    }

    void MoveNRotate(WheelCollider wc,float vertical, float horizontal, float space, float speedF, float speedR)
    {
        //проверяем ручник
        if (space == 0)
        {
            //едем вперёд или назад, без поворота
            if (vertical != 0 & horizontal == 0)
            {
                wc.brakeTorque = 0;//сила тормоза 
                wc.motorTorque = speedF * vertical;//задаём силу двигателю
            }
            else if (vertical == 0 & horizontal != 0)
            {
                //поворот на месте
                wc.brakeTorque = 0;
                wc.motorTorque = speedR * horizontal;
            }
            else if (vertical != 0 & horizontal != 0)
            {
                //движение  с поворотами
                wc.brakeTorque = 0;
                wc.motorTorque = speedF * horizontal;
            }
            else
            {
                //ручник, если не нажимаем на клавиши управления
                wc.brakeTorque = maxBrakeForce;
                wc.motorTorque = 0;
            }
        }
        else
        {
            wc.brakeTorque = maxBrakeForce*maxBrakeForce;
            wc.motorTorque = 0;
        }
    }
}
