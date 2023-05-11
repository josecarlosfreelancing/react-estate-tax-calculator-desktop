using System.Collections;
using System.Collections.Generic;
using EstateView.Core.Model;

namespace EstateView.ViewModel.Chart
{
    public class ProjectionsViewModel : ViewModel, IEnumerable<EstateProjection>
    {
        private IEnumerable<EstateProjection> projections;

        public ProjectionsViewModel(IEnumerable<EstateProjection> projections)
        {
            this.Bind(projections);
        }

        public void Bind(IEnumerable<EstateProjection> projections)
        {
            this.projections = projections;
        }

        public IEnumerator<EstateProjection> GetEnumerator()
        {
            return this.projections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
