﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrMario.Interfaces
{
    public interface IGameBlock
    {
        IGameCell Left { get; set; }
        IGameCell Right { get; set; }

        GameCellType Type { get; set; }

        void SetRandomColor();

        bool CanFallDown(IGameField field);
        void FallDown(IGameField field);

        bool CanTurn(IGameField field);

        bool CanMove(GameSide side, IGameField field);

        /// <summary>
        /// Повернуть блок (направо)
        /// </summary>
        void Turn();

        /// <summary>
        /// Подвинуть блок
        /// </summary>
        void Move(GameSide side);
    }
}
