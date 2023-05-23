using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    List<string> distinct_SoundList = new List<string>();
    [SerializeField] List<Sound> sfx_List = new List<Sound>();
    //[SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;
    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;
    private void Start()
    { 
        PlayBGM("Kirby");
    }

    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }
    public void StopSFX()
    {
        for (int j = 0; j < sfxPlayer.Length; j++)
        {
            // SFXPlayer���� ��� ������ ���� Audio Source�� �߰��ߴٸ� 
            if (sfxPlayer[j].isPlaying)
            {
                sfxPlayer[j].Stop();
                
            }
        }
    }
    public void LoadSound()
    {
        foreach (string str in ScriptManager.sound_List)
        {
            distinct_SoundList.Add(str);
        }
        distinct_SoundList = distinct_SoundList.Distinct().ToList();
        distinct_SoundList.RemoveAll(string.IsNullOrEmpty);
        foreach (string str in distinct_SoundList)
        {
            AudioClip clip = Resources.Load("Sounds/SFX/"+str, typeof(AudioClip))as AudioClip;
            Sound sound = new Sound();
            sound.name = str;
            sound.clip = clip;
            sfx_List.Add(sound); // xml���Ͽ� �ִ� sound�̸����� ã�Ƽ� SFXList�� ����.
           
        }
    }
    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx_List.Count; i++)
        {
            if (p_sfxName == sfx_List[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    // SFXPlayer���� ��� ������ ���� Audio Source�� �߰��ߴٸ� 
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx_List[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ����� �÷��̾ ������Դϴ�.");
                return;
            }
        }
        Debug.Log(p_sfxName + " �̸��� ȿ������ �����ϴ�.");
        return;
    }
    public void get_SFXName(string SFX_Name)
    {
        PlaySFX(SFX_Name);
    }
}

