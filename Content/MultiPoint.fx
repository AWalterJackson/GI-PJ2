// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Adapted for COMP30019 by Jeremy Nicholson, 10 Sep 2012
// Adapted further by Chris Ewin, 23 Sep 2013

// these won't change in a given iteration of the shader
// It is more efficient to use a set number of lights than to do this dynamically
#define MAX_LIGHTS 15

struct Light
{
    float4 lightPos;
    float4 lightCol;
};

float4x4 World;
float4x4 View;
float4x4 Projection;
float4 cameraPos;
float4x4 worldInvTrp;

float4 lightAmbCol;

Light l1;
Light l2;
Light l3;
Light l4;
Light l5;
Light l6;
Light l7;
Light l8;
Light l9;
Light l10;
Light l11;
Light l12;
Light l13;
Light l14;
Light l15;

struct VS_IN
{
	float4 pos : SV_POSITION;
	float4 nrm : NORMAL;
	float4 col : COLOR;
};

struct PS_IN
{
	float4 pos : SV_POSITION; //Position in camera co-ords
	float4 col : COLOR;       //Vertex Colour
	float4 wpos : TEXCOORD0;  //Position in world co-ords
	float3 wnrm : TEXCOORD1;  //Normal in world co-ords 
};


PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;

	// Convert Vertex position and corresponding normal into world coords
	// Note that we have to multiply the normal by the transposed inverse of the world 
	// transformation matrix (for cases where we have non-uniform scaling; we also don't
	// care about the "fourth" dimension, because translations don't affect the normal)
	output.wpos = mul(input.pos, World);
	output.wnrm = mul(input.nrm.xyz, (float3x3)worldInvTrp);

	// Transform vertex in world coordinates to camera coordinates
	float4 viewPos = mul(output.wpos, View);
    output.pos = mul(viewPos, Projection);

	// Just pass along the colour at the vertex
	output.col = input.col;

	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	// Our interpolated normal might not be of length 1
	float3 interpNormal = normalize(input.wnrm);

	// Calculate ambient RGB intensities
	float Ka = 1;
	float3 amb = input.col.rgb*lightAmbCol.rgb*Ka;

	float4 returnCol = float4(0.0f, 0.0f, 0.0f, 0.0f);
	returnCol.rgb = amb.rgb;
	returnCol.a = input.col.a;

	Light currentLight;
	for (int i = 0; i < MAX_LIGHTS; i++) {
		if (i == 0){ currentLight = l1; }
		if (i == 1){ currentLight = l2; }
		if (i == 2){ currentLight = l3; }
		if (i == 3){ currentLight = l4; }
		if (i == 4){ currentLight = l5; }
		if (i == 5){ currentLight = l6; }
		if (i == 6){ currentLight = l7; }
		if (i == 7){ currentLight = l8; }
		if (i == 8){ currentLight = l9; }
		if (i == 9){ currentLight = l10; }
		if (i == 10){ currentLight = l11; }
		if (i == 11){ currentLight = l12; }
		if (i == 12){ currentLight = l13; }
		if (i == 13){ currentLight = l14; }
		if (i == 14){ currentLight = l15; }
		// Calculate diffuse RBG reflections
		float fAtt = 1;
		float Kd = 1;
		float3 L = normalize(currentLight.lightPos.xyz - input.wpos.xyz);
		float LdotN = saturate(dot(L, interpNormal.xyz));
		float3 dif = fAtt*currentLight.lightCol.rgb*Kd*input.col.rgb*LdotN;

		// Calculate specular reflections
		float Ks = 1;
		float specN = 5; // Numbers>>1 give more mirror-like highlights
		float3 V = normalize(cameraPos.xyz - input.wpos.xyz);
		//float3 R = normalize(2 * LdotN*interpNormal.xyz - L.xyz);
		float3 R = normalize(0.5*(L.xyz+V.xyz)); //Blinn-Phong equivalent
		float3 spe = fAtt*currentLight.lightCol.rgb*Ks*pow(saturate(dot(V, R)), specN);
		returnCol.rgb = returnCol.rgb + dif.rgb + spe.rgb;
	}

	return returnCol;
}



technique Lighting
{
    pass Pass1
    {
		// TASK 6 - One simple way to accomplish this is to target a newer shader profile
		Profile = 10.0;
        VertexShader = VS;
        PixelShader = PS;
    }
}