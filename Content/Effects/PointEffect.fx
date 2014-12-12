// Parameters that should be set from the program
float4x4 World; // World Matrix
float4x4 View; // View Matrix
float4x4 Projection; // Projection Matrix
float3 CameraPosition; // in world space
float4 DiffuseColor;
texture PointTexture;
float2 Tiling;
float2 Offset;

sampler PointSampler = sampler_state
{
	Texture = <PointTexture>;
	AddressU = Clamp;
	AddressV = Clamp;
};

// We create structs to help us manage the inputs/outputs
// to vertex and pixel shaders
struct VertexInput
{
    float4 Position : POSITION0; // Here, POSITION0 and NORMAL0
	float3 Normal : NORMAL0; // are called mnemonics
	float2 UV: TEXCOORD0;
};

// --- Per Pixel techniques - Phong, Blinn, Schlick
// We will need a different output struct for this
struct PhongVertexOutput
{
	float4 Position : POSITION0;
	float2 UV: TEXCOORD0;
};
// A common vertex shader for all the different techniques
PhongVertexOutput PhongVertex(VertexInput input)
{
	PhongVertexOutput output;
	output.Position = mul(mul(mul(input.Position, World), View), Projection);
	output.UV = input.UV * Tiling + Offset;
	return output;
}
// The pixel shader performs the lighting
float4 PhongPixel(PhongVertexOutput input) : COLOR0
{
	return DiffuseColor *tex2D(PointSampler, input.UV);
}

technique Phong
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 PhongVertex();
		PixelShader = compile ps_2_0 PhongPixel();
	}
}
