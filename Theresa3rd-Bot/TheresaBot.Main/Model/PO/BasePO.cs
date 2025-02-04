﻿using SqlSugar;

namespace TheresaBot.Main.Model.PO
{
    public record BasePO
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键,自增")]
        public int Id { get; set; }

        public BasePO() { }
    }
}
