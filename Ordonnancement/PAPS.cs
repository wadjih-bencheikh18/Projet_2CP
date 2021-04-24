namespace Ordonnancement
{
    class PAPS : Ordonnancement
    {
        public void Executer(int tempsDebut, int tempsFin)
        {

            SortListeProcessus();
            int temps = tempsDebut, indice = 0;
            while ((indice < listeProcessus.Count || listeExecution.Count != 0) && temps < tempsFin) //s'il existe des processus non executés et le temps < le temps de fin
            {
                indice = AjouterTous(temps, indice); //remplir listeExecution
                temps++; //incrementer le temps réel
                if (listeExecution.Count != 0) //s'il y a des processus à executer
                {
                    listeExecution[0].tempsRestant--; //l'execution du 1er processus de listeExecution commence
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++; //incrementer le temps d'attente des processus de listeExecution à partir du 2eme 
                    if (listeExecution[0].tempsRestant == 0) //si l'execution du premier processus de listeExecution est terminée
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
            }

        }
        public int AjouterTous(int temps, int indice) //ajouter à la liste d'execution tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
        {
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else listeExecution.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste d'execution et on passe au suivant
            }
            return indice;
        }
    }
}