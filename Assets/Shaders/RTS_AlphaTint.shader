Shader "RTS_Engine/RTS_AlphaTint" 
{     

Properties 
{ 
   _Color ("Tint Color", Color) = (1,1,1) 
   _MainTex ("Texture", 2D) = "white"
}  

SubShader
{   
    Pass
    {
        SetTexture [_MainTex] 
        {
            ConstantColor [_Color]
            combine texture * constant 
        }  

        SetTexture [_MainTex] 

        {
            combine texture lerp(texture) previous
        }
    }
} 
}