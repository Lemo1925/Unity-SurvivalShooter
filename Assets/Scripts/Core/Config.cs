using System.Runtime.Serialization;

[DataContract]
public class Config
{
    [DataMember]
    public float MusicVolume;
    [DataMember]
    public float EffectVolume;
   
    public Config(float MusicVolume = 1.0f, float EffectVolume = 1.0f)
    {
        this.MusicVolume = MusicVolume;
        this.EffectVolume = EffectVolume;
    }

    public override string ToString() => $"Music:{MusicVolume},Effect:{EffectVolume}";
}
