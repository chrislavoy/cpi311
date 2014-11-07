// Parameters that should be set from the program
float4x4 World; // World Matrix
float4x4 View; // View Matrix
float4x4 Projection; // Projection Matrix
float3 LightPosition; // in world space
float3 CameraPosition; // in world space
float Shininess; // scalar value
float3 AmbientColor;
float3 DiffuseColor;
float3 SpecularColor;
texture NormalMap;


sampler NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
	AddressU = Wrap;
	AddressV = Wrap;
};

// We create structs to help us manage the inputs/outputs
// to vertex and pixel shaders
struct VertexInput
{
    float4 Position : POSITION0; // Here, POSITION0 and NORMAL0
	float2 UV: TEXCOORD0;
};

struct PhongVertexOutput
{
	float4 Position : POSITION0;
	float2 UV: TEXCOORD0;
	float4 WorldPosition : TEXCOORD1;
};
// A common vertex shader for all the different techniques
PhongVertexOutput PhongVertex(VertexInput input)
{
	PhongVertexOutput output;
	// Do the transformations as before
	// Save the world position for use in the pixel shader
	output.WorldPosition = mul(input.Position, World);
	float4 viewPosition = mul(output.WorldPosition, View);
	output.Position = mul(viewPosition, Projection);
	// as well as the normal in world space
	output.UV = input.UV;
	return output;
}
// The pixel shader performs the lighting
float4 PhongPixel(PhongVertexOutput input) : COLOR0
{
	// The lighting operation, same as in the Gouraud vertex method
	float3 lightDirection = normalize(LightPosition - input.WorldPosition.xyz);
	float3 viewDirection = normalize(CameraPosition - input.WorldPosition.xyz);
	// Need to normalize my incoming normal, length could be less than 1
	float3 normal = normalize(mul(tex2D(NormalMapSampler, input.UV).xyz, World));
	float3 reflectDirection = -reflect(lightDirection, normal);
	// Now, compute the lighint components
	float diffuse = max(dot(lightDirection, normal), 0);
	float specular = pow(max(dot(reflectDirection, viewDirection), 0), Shininess);
	return float4(AmbientColor + diffuse * DiffuseColor + specular * SpecularColor, 1);
}

technique Phong
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 PhongVertex();
		PixelShader = compile ps_2_0 PhongPixel();
	}
}
