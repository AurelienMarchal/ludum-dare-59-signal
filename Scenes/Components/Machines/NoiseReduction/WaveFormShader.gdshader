shader_type spatial;

const int MAX_WAVES = 8;

uniform int wave_count = 3;
uniform float frequencies[MAX_WAVES];
uniform float amplitudes[MAX_WAVES];
uniform float phases[MAX_WAVES];

uniform float thickness = 0.02;

// 0 = clean, 1 = glitch
uniform float noise_amount : hint_range(0.0, 1.0) = 0.0;

// glitch controls
uniform float glitch_scale = 40.0;
uniform float glitch_speed = 15.0;
uniform float glitch_strength = 0.4;

// hash function
float hash(float x) {
    return fract(sin(x * 127.1) * 43758.5453);
}

// stepped glitch noise
float glitch_noise(float x) {
    float t = floor(TIME * glitch_speed);
    return hash(floor(x) + t);
}

void fragment() {
    float t = TIME;

    // --- CLEAN MULTI-WAVE ---
    float clean_wave = 0.0;

    for (int i = 0; i < wave_count; i++) {
        float phase = phases[i] + t;
        clean_wave += sin(UV.x * frequencies[i] * 6.28318 + phase) * amplitudes[i];
    }

    // optional: keep it in bounds
    clean_wave = clamp(clean_wave, -0.5, 0.5);

    // --- GLITCH NOISE ---
    float n = glitch_noise(UV.x * glitch_scale);

    float glitch_wave = clean_wave;

    // amplitude distortion
    glitch_wave *= mix(1.0 - glitch_strength, 1.0 + glitch_strength, n);

    // vertical jitter
    glitch_wave += (n - 0.5) * glitch_strength;

    // --- BLEND CLEAN ↔ GLITCH ---
    float wave = mix(clean_wave, glitch_wave, noise_amount);

    float y = wave + 0.5;

    float dist = abs(UV.y - y);
    float line = smoothstep(thickness, 0.0, dist);

    ALBEDO = vec3(line);
}