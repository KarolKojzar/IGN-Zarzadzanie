using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace IGN_Zarzadzanie
{
    class Zmienne
    {
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public DataTable dt
        {
            get { return _dt; }
            set { _dt= value; }
        }


        private int row = -1;
        private DataTable _dt;

        private MySqlConnection Polaczenie;

        public MySqlConnection polaczenie
        {
            get { return Polaczenie; }
            set { Polaczenie = value; }
        }
    }
}
