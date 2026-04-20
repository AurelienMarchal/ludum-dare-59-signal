shader_type spatial;
render_mode unshaded, cull_disabled;

uniform sampler2D viewport_tex : source_color, repeat_enable, filter_nearest;
uniform vec2 scroll_speed = vec2(0.1, 0.0);

void fragment() {
    vec2 uv = UV + scroll_speed * TIME;
    uv = fract(uv);

    vec4 col = texture(viewport_tex, uv);
    ALBEDO = col.rgb;
    ALPHA = col.a;
}
