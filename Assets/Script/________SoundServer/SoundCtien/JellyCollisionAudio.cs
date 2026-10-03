using UnityEngine;

public class JellyCollisionAudio : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource audioSource;
    public AudioClip[] squishSounds; // Mảng chứa các file tiếng "squish/jelly"

    [Header("Collision Settings")]
    public float minImpactForce = 1.5f; // Lực tối thiểu mới phát tiếng
    public float cooldownTime = 0.15f;  // Thời gian chờ giữa 2 lần phát

    private float lastPlayTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra cooldown
        if (Time.time - lastPlayTime < cooldownTime) return;

        // Tính lực va chạm tương đối
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= minImpactForce)
        {
            PlaySquishSound(impactForce);
            lastPlayTime = Time.time;
        }
    }

    private void PlaySquishSound(float force)
    {
        if (squishSounds.Length == 0 || audioSource == null) return;

        // Chọn ngẫu nhiên 1 clip tiếng thạch
        AudioClip clip = squishSounds[Random.Range(0, squishSounds.Length)];

        // Đổi Pitch ngẫu nhiên một chút cho âm thanh không bị lặp lại thô cứng
        audioSource.pitch = Random.Range(0.85f, 1.15f);

        // Chỉnh Volume theo lực va chạm (chạm nhẹ tiếng nhỏ, chạm mạnh tiếng to)
        float volume = Mathf.Clamp01(force / 10f); 

        audioSource.PlayOneShot(clip, volume);
    }
}