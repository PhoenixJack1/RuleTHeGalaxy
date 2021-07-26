using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    /// <summary> Совокупность эффектов наложенных на корабль </summary>
    public class ShipStatus
    {
        /// <summary> Ослепление, снижение точности на 30 единиц</summary>
        public byte IsBlinded = 0; //0 - эффекта нет, 1 - только текущий ход, 2 - также следующий ход
        /// <summary> Замедление, снижение уклонения на 30 единиц </summary>
        //public byte IsSlowed = 0;
        /// <summary> Сотрясение, корабль не может выполнять действия </summary>
        public byte IsConfused = 0;
        /// <summary> Проклятие от крита от антипушки </summary>
        public byte IsAntiCursed = 0;
        /// <summary> Корабль получает эффект движения - уклонение увеличивается на 30 </summary>
        public byte IsMoving = 0;
        /// <summary> Метка - снижение уклонения на 50 едининц </summary>
        public byte IsMarked = 0;
        /// <summary> Проклятие от крита Пси пушки </summary>
        public byte IsPsiCursed = 0;
        public ShipStatus()
        {

        }
        /// <summary> Конец раунда, все эффекты с ограниченным сроком уменьшают длительность </summary>
        public void RoundEnd()
        {
            if (IsBlinded > 0) IsBlinded--;
            if (IsMarked > 0) IsMarked--;
            //if (IsSlowed > 0) IsSlowed--;
            if (IsConfused > 0) IsConfused--;
            if (IsAntiCursed > 0) IsAntiCursed--;
            if (IsPsiCursed > 0) IsPsiCursed--;
            if (IsMoving > 0) IsMoving = 0;
        }
        /// <summary> Корабль получает эффект ослепления </summary>
        public void SetBlind()
        {
            IsBlinded = 2;
        }
        ///<summary>На корабль ставят метку</summary>
        public void SetMark()
        {
            IsMarked = 2;
        }
        /// <summary> Корабль получает эффект замедления </summary>
      /*  public void SetSlow()
        {
            IsSlowed = 2;
        }*/
        
        /// <summary> Корабль получает эффект сотрясения </summary>
        public void SetConfuse()
        {
            IsConfused = 2;
        }
        /// <summary> Корабль получает эффект проклятия от антипушки </summary>
        public void SetAntiCursed()
        {
            IsAntiCursed = 2;
        }
        /// <summary> Корабль получает эффект проклятия от псипушки </summary>
        public void SetPsiCursed()
        {
            IsPsiCursed = 2;
        }
        /// <summary> Корабль получает эффект движения </summary>
        public void SetMoving()
        {
            IsMoving = 1;
        }
        /// <summary> Снимает все негативные эффекты с корабля </summary>
        public void Clear()
        {
            IsBlinded = 0;
            IsMarked = 0;
            //IsSlowed = 0;
            IsConfused = 0;
            IsAntiCursed = 0;
            IsPsiCursed = 0;
        }
    }
}
