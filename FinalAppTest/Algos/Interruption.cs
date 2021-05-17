namespace Ordonnancement
{
    public class Interruption
    {
        public string type { get; set; }
        public int duree { get; set; }
        public int tempsRestant { get; set; }
        public int tempsArriv { get; set; }

        #region Contructeur

        /// <summary>
        /// initialiser une Interruption
        /// </summary>

        public Interruption(string type, int duree, int tempsArriv)
        {
            this.type = type;
            this.duree = duree;
            tempsRestant = duree;
            this.tempsArriv = tempsArriv;
        }
        public Interruption(int duree, int tempsArriv)
        {
            this.duree = duree;
            tempsRestant = duree;
            this.tempsArriv = tempsArriv;
        }
        #endregion

        #region Executer

        /// <summary>
        /// Executer l'interruption
        /// </summary>

        public bool Prete(int temps)
        {
            if (tempsArriv == temps) return true;
            else return false;
        }
        public void Execute()
        {
            tempsRestant--;
        }
        #endregion
    }
}