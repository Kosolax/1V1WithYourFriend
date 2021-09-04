Shader "Outlined" {
	Properties{
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0, 5)) = .1
		_MainTex("Base (RGB)", 2D) = "white" { }
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	// défini la gueule de notre objet de base
	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	// crée notre "objet" pour les vertex
	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	// récup les valeurs des propriétés
	uniform float _Outline;
	uniform float4 _OutlineColor;

	// Défini la méthode qui choppe les vertex et les multiplies
	v2f vert(appdata v) {

		v2f o;
		v.vertex *= _Outline;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.color = _OutlineColor;
		return o;
	}

	// Le fragment sur un objet c'est un script qui run sur chacun des pixels de l'objet
	// donc ici comme on a pris les vertex qu'on les a copié et multiplié et changer leurs couleurs
	// quand il va passer sur tout les pixels ca va reconstruire l'objet + reconstruire les nouveaux vertex
	// et donc dessiner les lignes autour de l'objet
	half4 frag(v2f i) :COLOR{ return i.color; }
	ENDCG

	SubShader{
		CGPROGRAM
		// c'est le shader qui fait les surface à partir des lights
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		// dit au shader comment il doit dessiner les surfaces
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG

		Pass {
			// cull front ça coupe que les faces devant la caméra (ça permet un détourage des arrêtes sur le côté et pas celle qu'on voit)
			Cull Front

			CGPROGRAM
			// appel la méthode des vertex en passant en param l'objet de base je crois
			#pragma vertex vert

			// appel la méthode pour dessiner l'objet
			#pragma fragment frag
			ENDCG
		}
	}

Fallback "Diffuse"
}