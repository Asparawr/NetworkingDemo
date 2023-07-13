using System.Collections;
using System.Collections.Generic;
using Mirror.Discovery;
using TMPro;
using UnityEngine;

namespace Mirror.Discovery
{
    public class ServerEntry : MonoBehaviour
    {
        //textmeshpro
        public TextMeshProUGUI ServerName;

        public void Populate(ServerResponse info)
        {
            ServerName.text = info.uri.Host.ToString();
        }
    }
}