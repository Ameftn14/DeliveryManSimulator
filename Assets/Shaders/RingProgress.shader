
Shader "RingProgress"
{
	Properties
	{
	 [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	 _Color("Tint", Color) = (1,1,1,1)
	 [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	 _Fill("Fill",Range(-1,1)) = 0
	}

		SubShader
	 {
		 Tags
		 {
			 "Queue" = "Transparent"
			 "IgnoreProjector" = "True"
			 "RenderType" = "Transparent"
			 "PreviewType" = "Plane"
			 "CanUseSpriteAtlas" = "True"
		 }

		 Cull Off
		 Lighting Off
		 ZWrite Off
		 Fog {Mode Off}
		 Blend SrcAlpha OneMinusSrcAlpha

		 Pass{
		 CGPROGRAM
		 #pragma vertex vert		 
		 #pragma fragment frag	
		 #pragma multi_compile DUMMY PIXELSNAP_ON
		  #include "UnitySprites.cginc"

	   float _Fill;


	   v2f vert(appdata_t IN)
	   {
		   v2f OUT;
		   OUT.vertex = UnityObjectToClipPos(IN.vertex);
		   OUT.texcoord = IN.texcoord;
		   OUT.color = IN.color*_Color;
		   #ifdef PIXELSNAP_ON
		   OUT.vertex = UnityPixelSnap(OUT.vertex);
		   #endif
		   return OUT;
	   }


	   fixed4 frag(v2f IN) :COLOR
	   {
		   fixed4 result = tex2D(_MainTex,IN.texcoord)*IN.color;
		   fixed2 p = fixed2(IN.texcoord.x - 0.5,IN.texcoord.y - 0.5);

		   if (_Fill < 0.5 && _Fill >= 0)
		   {
			//    float compare = (_Fill * 2 - 0.5)*3.1415926;
			//    float theta = atan(p.y / p.x);
			//    if (theta > compare)
			//    {
			// 	   result.a = 0;
			//    }
			//    if (p.x > 0)
			//    {
			//    result.a = 0;
			//    }
			//改成逆时针
			   float compare = (0.5 - _Fill * 2)*3.1415926;
			   float theta = atan(p.y / p.x);
			   if (p.x < 0)
			   {
				   result.a = 0;
			   }
			   if (theta < compare)
			   {
				   result.a = 0;
			   }
		   }
		   else if(_Fill >= 0.5 && _Fill <= 1)
		   {
			//    float compare = ((_Fill - 0.5) * 2 - 0.5)*3.1415926;
			//    float theta = atan(p.y / p.x);
			//    if (p.x > 0)
			//    {
			// 	   if (theta > compare)
			// 	   {
			// 	   result.a = 0;
			// 	   }
			//    }
			    float compare = ( 0.5 - (_Fill - 0.5) * 2)*3.1415926;
				float theta = atan(p.y / p.x);
				if (p.x < 0){
					if (theta < compare)
					{
						result.a = 0;
					}
				}
											

		   }
		   else if(_Fill > -0.5 && _Fill < 0)
		   {
			   float tempfill = 1+_Fill;
			   float compare = ( 0.5 - (tempfill - 0.5) * 2)*3.1415926;
			   float theta = atan(p.y / p.x);
				if (p.x < 0){
					if (theta > compare)
					{
						result.a = 0;
					}
				}
				else{
					result.a = 0;
				}
		   }
		   else if(_Fill <= -0.5 && _Fill >= -1)
		   {
			   float tempfill = 1+_Fill;
			   float compare = (0.5 - tempfill * 2)*3.1415926;
			   float theta = atan(p.y / p.x);
			   if (p.x > 0)
			   {
				   if (theta > compare)
				   {
					   result.a = 0;
				   }
	           }
		   }
		   return result;
	   }
	   ENDCG
	   }
	 }
		 Fallback "Transparent/VertexLit"
}

