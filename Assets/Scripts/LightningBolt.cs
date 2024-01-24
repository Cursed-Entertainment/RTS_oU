using UnityEngine;

public class LightningBolt : MonoBehaviour
{
	public Transform target;
	private Vector3 targetVector;
	public int zigs = 100;
	public float speed = 1f;
	public float scale = 1f;
	public Light startLight;
	public Light endLight;
	public Vector3 targetPos = Vector3.zero;
	
	Perlin noise;
	float oneOverZigs;
	
	private Particle[] particles;
	
	void Start()
	{
		oneOverZigs = 1f / (float)zigs;
		GetComponent<ParticleEmitter>().emit = false;

		GetComponent<ParticleEmitter>().Emit(zigs);
		particles = GetComponent<ParticleEmitter>().particles;
	}
	
	public void Update ()
	{
		if (target != null || targetPos != Vector3.zero)
		{
			if (targetPos != Vector3.zero) targetVector = targetPos;
			else targetVector = target.position;
			if (noise == null)
				noise = new Perlin();
			
			float timex = Time.time * speed * 0.1365143f;
			float timey = Time.time * speed * 1.21688f;
			float timez = Time.time * speed * 2.5564f;		
		
			for (int i=0; i < particles.Length; i++)
			{
				Vector3 position = Vector3.Lerp(transform.position, targetVector, oneOverZigs * (float)i);
				Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
											noise.Noise(timey + position.x, timey + position.y, timey + position.z),
											noise.Noise(timez + position.x, timez + position.y, timez + position.z));
				position += (offset * scale * ((float)i * oneOverZigs));
			
				particles[i].position = position;
				particles[i].color = Color.white;
				particles[i].energy = 1f;
			}
		
			GetComponent<ParticleEmitter>().particles = particles;
		
			if (GetComponent<ParticleEmitter>().particleCount >= 2)
			{
				if (startLight)
					startLight.transform.position = particles[0].position;
				if (endLight)
					endLight.transform.position = particles[particles.Length - 1].position;
			}
		}
	}	
}