Shader "Hidden/ProBuilder/VertexPicker" {
	Properties {
	}
	SubShader {
		Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "ProBuilderPicker" = "VertexPass" "RenderType" = "Transparent" }
		Pass {
			Name "VERTICES"
			Tags { "DisableBatching" = "true" "IGNOREPROJECTOR" = "true" "ProBuilderPicker" = "VertexPass" "RenderType" = "Transparent" }
			ZClip Off
			Cull Off
			Offset -1, -1
			GpuProgramID 18885
			Program "vp" {
				SubProgram "d3d9 " {
					"!!vs_3_0
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 10.1
					//
					// Parameters:
					//
					//   float4 _ScreenParams;
					//   row_major float4x4 glstate_matrix_projection;
					//   row_major float4x4 unity_MatrixV;
					//   row_major float4x4 unity_ObjectToWorld;
					//
					//
					// Registers:
					//
					//   Name                      Reg   Size
					//   ------------------------- ----- ----
					//   unity_ObjectToWorld       c0       4
					//   glstate_matrix_projection c4       4
					//   unity_MatrixV             c8       3
					//   _ScreenParams             c11      1
					//
					
					    vs_3_0
					    def c12, 1, 0, 0.949999988, 0.5
					    def c13, 3.5, 9.99999975e-005, 0, 0
					    dcl_position v0
					    dcl_color v1
					    dcl_texcoord v2
					    dcl_texcoord1 v3
					    dcl_position o0
					    dcl_texcoord o1.xy
					    dcl_color o2
					    mov r0.x, c12.x
					    add r0.x, r0.x, -c7.w
					    mad r1, v0.xyzx, c12.xxxy, c12.yyyx
					    dp4 r2.x, c0, r1
					    dp4 r2.y, c1, r1
					    dp4 r2.z, c2, r1
					    dp4 r2.w, c3, r1
					    dp4 r1.x, c8, r2
					    dp4 r1.y, c9, r2
					    dp4 r1.z, c10, r2
					    mul r1.xyz, r1, c12.z
					    mov r1.w, c12.x
					    dp4 r0.y, c6, r1
					    mad r3.z, r0.x, -c13.y, r0.y
					    dp4 r0.x, c4, r1
					    dp4 r0.y, c5, r1
					    dp4 r0.z, c7, r1
					    rcp r0.w, r0.z
					    mul r0.xy, r0.w, r0
					    mad r0.xy, r0, c12.w, c12.w
					    mul r1.xy, c13.x, v3
					    mad r0.xy, r0, c11, r1
					    rcp r1.x, c11.x
					    rcp r1.y, c11.y
					    mad r0.xy, r0, r1, -c12.w
					    mul r0.xy, r0.z, r0
					    mov r3.w, r0.z
					    add r3.xy, r0, r0
					    mov o1.xy, v2
					    mov o2, v1
					    mad o0.xy, r3.w, c255, r3
					    mov o0.zw, r3
					
					// approximately 32 instruction slots used"
				}
				SubProgram "d3d11 " {
					"!!vs_4_0
					//
					// Generated by Microsoft (R) D3D Shader Disassembler
					//
					//
					// Input signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// POSITION                 0   xyzw        0     NONE   float   xyz 
					// NORMAL                   0   xyz         1     NONE   float       
					// COLOR                    0   xyzw        2     NONE   float   xyzw
					// TEXCOORD                 0   xy          3     NONE   float   xy  
					// TEXCOORD                 1   xy          4     NONE   float   xy  
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_POSITION              0   xyzw        0      POS   float   xyzw
					// TEXCOORD                 0   xy          1     NONE   float   xy  
					// COLOR                    0   xyzw        2     NONE   float   xyzw
					//
					vs_4_0
					dcl_constantbuffer CB0[7], immediateIndexed
					dcl_constantbuffer CB1[4], immediateIndexed
					dcl_constantbuffer CB2[13], immediateIndexed
					dcl_input v0.xyz
					dcl_input v2.xyzw
					dcl_input v3.xy
					dcl_input v4.xy
					dcl_output_siv o0.xyzw, position
					dcl_output o1.xy
					dcl_output o2.xyzw
					dcl_temps 2
					mul r0.xyzw, v0.yyyy, cb1[1].xyzw
					mad r0.xyzw, cb1[0].xyzw, v0.xxxx, r0.xyzw
					mad r0.xyzw, cb1[2].xyzw, v0.zzzz, r0.xyzw
					add r0.xyzw, r0.xyzw, cb1[3].xyzw
					mul r1.xyz, r0.yyyy, cb2[10].xyzx
					mad r1.xyz, cb2[9].xyzx, r0.xxxx, r1.xyzx
					mad r0.xyz, cb2[11].xyzx, r0.zzzz, r1.xyzx
					mad r0.xyz, cb2[12].xyzx, r0.wwww, r0.xyzx
					mul r0.xyz, r0.xyzx, l(0.950000, 0.950000, 0.950000, 0.000000)
					mul r1.xyzw, r0.yyyy, cb2[6].xyzw
					mad r1.xyzw, cb2[5].xyzw, r0.xxxx, r1.xyzw
					mad r0.xyzw, cb2[7].xyzw, r0.zzzz, r1.xyzw
					add r0.xyzw, r0.xyzw, cb2[8].xyzw
					div r0.xy, r0.xyxx, r0.wwww
					mad r0.xy, r0.xyxx, l(0.500000, 0.500000, 0.000000, 0.000000), l(0.500000, 0.500000, 0.000000, 0.000000)
					mul r1.xy, v4.xyxx, l(3.500000, 3.500000, 0.000000, 0.000000)
					mad r0.xy, r0.xyxx, cb0[6].xyxx, r1.xyxx
					div r0.xy, r0.xyxx, cb0[6].xyxx
					add r0.xy, r0.xyxx, l(-0.500000, -0.500000, 0.000000, 0.000000)
					mul r0.xy, r0.wwww, r0.xyxx
					add o0.xy, r0.xyxx, r0.xyxx
					add r0.x, -cb2[8].w, l(1.000000)
					mad o0.z, -r0.x, l(0.000100), r0.z
					mov o0.w, r0.w
					mov o1.xy, v3.xyxx
					mov o2.xyzw, v2.xyzw
					ret 
					// Approximately 0 instruction slots used"
				}
			}
			Program "fp" {
				SubProgram "d3d9 " {
					"!!ps_3_0
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 10.1
					    ps_3_0
					    dcl_color_pp v0
					    mov_pp oC0, v0
					
					// approximately 1 instruction slot used"
				}
				SubProgram "d3d11 " {
					"!!ps_4_0
					//
					// Generated by Microsoft (R) D3D Shader Disassembler
					//
					//
					// Input signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_POSITION              0   xyzw        0      POS   float       
					// TEXCOORD                 0   xy          1     NONE   float       
					// COLOR                    0   xyzw        2     NONE   float   xyzw
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_Target                0   xyzw        0   TARGET   float   xyzw
					//
					ps_4_0
					dcl_input_ps linear v2.xyzw
					dcl_output o0.xyzw
					mov o0.xyzw, v2.xyzw
					ret 
					// Approximately 0 instruction slots used"
				}
			}
		}
	}
}