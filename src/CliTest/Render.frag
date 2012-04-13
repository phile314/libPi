
#version 120

varying vec2	v_texPos;

uniform sampler2D	u_texture0;


void main() {
	gl_FragColor = texture2D(u_texture0, v_texPos);
}