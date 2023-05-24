#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Properties of shader
float2 texelSize = float2(.0f, .0f);
float4 outlineColor = float4(.0f, .0f, .0f, .0f);

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state {
    Texture = <SpriteTexture>;
};

// This data comes from the sprite batch vertex shader
struct VertexShaderOutput {
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// Our pixel shader
// Credit original code: https://community.monogame.net/t/how-do-i-use-shaders-to-create-border-around-a-sprite/11323/6
float4 MainPS(VertexShaderOutput input) : COLOR {
    float2 pos = float2(input.TextureCoordinates.x, input.TextureCoordinates.y);
    float4 col = tex2D(SpriteTextureSampler, pos);

    float center = ceil(col.a);

    pos = float2(input.TextureCoordinates.x - texelSize.x, input.TextureCoordinates.y);
    float left = ceil(tex2D(SpriteTextureSampler, pos).a) * ceil(pos.x);

    pos = float2(input.TextureCoordinates.x + texelSize.x, input.TextureCoordinates.y);
    float right = ceil(tex2D(SpriteTextureSampler, pos).a) * 1.0f - floor(pos.x);

    pos = float2(input.TextureCoordinates.x, input.TextureCoordinates.y - texelSize.y);
    float up = ceil(tex2D(SpriteTextureSampler, pos).a) * ceil(pos.y);

    pos = float2(input.TextureCoordinates.x, input.TextureCoordinates.y + texelSize.y);
    float down = ceil(tex2D(SpriteTextureSampler, pos).a) * 1.0f - floor(pos.y);

    float total = left + right + up + down;
    if (center > 0.0f && total < 4.0f)
        col = outlineColor;

    return col;
}

// Compile our shader
technique SpriteDrawing {
    pass P0 {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};