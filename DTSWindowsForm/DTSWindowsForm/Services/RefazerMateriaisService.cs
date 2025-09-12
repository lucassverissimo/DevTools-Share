using System.Collections.Generic;
using System.Linq;

namespace DTSWindowsForm.Services
{
    /// <summary>
    /// Provides operations for managing "materiais para refazer".
    /// </summary>
    public class RefazerMateriaisService
    {
        private List<MateriaisRefazer>? refazerMateriais;

        /// <summary>
        /// Ensures that the internal list is initialized.
        /// </summary>
        public void Initialize()
        {
            refazerMateriais ??= new List<MateriaisRefazer>();
        }

        /// <summary>
        /// Locates the material to redo for a given project and position.
        /// Safely handles missing list or item to avoid NullReferenceException.
        /// </summary>
        public void ListMateriaisRefazer(Projeto projeto, string posicao)
        {
            if (refazerMateriais == null) return;
            var sucataRefazer = refazerMateriais
                .FirstOrDefault(x => x.Projeto == projeto && x.Posicao == posicao);
            if (sucataRefazer == null) return;

            // existing logic …
        }
    }

    public class MateriaisRefazer
    {
        public Projeto Projeto { get; set; } = null!;
        public string Posicao { get; set; } = string.Empty;
    }

    public class Projeto { }
}
