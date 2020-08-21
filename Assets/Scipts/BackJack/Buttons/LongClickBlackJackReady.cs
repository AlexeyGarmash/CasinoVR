using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scipts.BackJack.Buttons
{
    public class LongClickBlackJackReady : LongClickProgerssBase
    {
        [PunRPC]
        public override void InvokeClickOut_RPC()
        {
            inGame = false;
            //_imageReady.texture = _notReadyTexture;
            _textReady.text = "Skip";
        }


        [PunRPC]
        public override void InvokeClickIn_RPC(string nickname)
        {
            inGame = true;
            //_imageReady.texture = _readyTexture;
            _textReady.text = nickname;
        }
        protected override void InvokeClickOut()
        {

            p_place.ps = lastCollider.GetComponentInParent<PlayerStats>();
            _onLongClickIn?.Invoke(p_place.ps);

            //photonView?.RequestOwnership();
            _onLongClickIn?.Invoke(playerStats);

            inGame = false;
            //photonView?.RPC("InvokeClickOut_RPC", RpcTarget.All);

        }

        protected override void InvokeClickIn()
        {
            //photonView?.RequestOwnership();
            _onLongClickIn?.Invoke(playerStats);
            inGame = true;
            //_imageReady.texture = _readyTexture;
            //_textReady.text = PhotonNetwork.LocalPlayer.NickName;
            //photonView?.RPC("InvokeClickIn_RPC", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);

        }
    }
}
