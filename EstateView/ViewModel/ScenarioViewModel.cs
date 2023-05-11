using System;
using System.Linq;
using System.Timers;
using EstateView.Core.Model.Scenarios;
using EstateView.ViewModel.Chart;
using EstateView.ViewModel.Logistics;

namespace EstateView.ViewModel
{
    public class ScenarioViewModel : ViewModel
    {
        private EstatePlanningScenario scenario;
        private readonly Timer timerToThrottleOptionsUpdates;

        public ScenarioViewModel(EstatePlanningScenario scenario)
        {
            this.timerToThrottleOptionsUpdates = this.CreateTimer();

            this.Bind(scenario);
        }

        public string Name
        {
            get
            {
                return this.scenario.Name;
            }
            set
            {
                this.scenario.Name = value;
                this.NotifyPropertyChanged(() => this.Name);
            }
        }

        public OptionsViewModel Options
        {
            get { return this.GetValue(() => this.Options); }
            private set { this.SetValue(() => this.Options, value); }
        }

        public OptionsViewModel ScenarioOptions
        {
            get { return this.GetValue(() => this.ScenarioOptions); }
            private set { this.SetValue(() => this.ScenarioOptions, value); }
        }

        public ProjectionsViewModel Projections
        {
            get { return this.GetValue(() => this.Projections); }
            private set { this.SetValue(() => this.Projections, value); }
        }

        public ProjectionsViewModel FilteredProjections
        {
            get { return this.GetValue(() => this.FilteredProjections); }
            private set { this.SetValue(() => this.FilteredProjections, value); }
        }

        public AccountsViewModel Accounts
        {
            get { return this.GetValue(() => this.Accounts); }
            private set { this.SetValue(() => this.Accounts, value); }
        }

        public ChartViewModel ChartBars
        {
            get { return this.GetValue(() => this.ChartBars); }
            private set { this.SetValue(() => this.ChartBars, value); }
        }

        public BoxesViewModel Boxes
        {
            get { return this.GetValue(() => this.Boxes); }
            private set { this.SetValue(() => this.Boxes, value); }
        }

        public bool IsSelected
        {
            get
            {
                return this.GetValue(() => this.IsSelected);
            }
            set
            {
                this.SetValue(() => this.IsSelected, value);

                if (value)
                {
                    this.HandleTimerElapsed(null, null);
                }
            }
        }

        public EstatePlanningScenario Scenario
        {
            get { return this.scenario; }
        }

        public void Bind(EstatePlanningScenario scenario)
        {
            if (this.Options != null)
            {
                this.Options.PropertyChanged -= this.HandleOptionsChanged;
            }
            
            this.scenario = scenario;
            this.Options = new OptionsViewModel(scenario.OriginalOptions);
            this.ScenarioOptions = new OptionsViewModel(scenario.Options);
            this.HandleTimerElapsed(null, null);

            this.Options.PropertyChanged += this.HandleOptionsChanged;
        }

        private Timer CreateTimer()
        {
            Timer timer = new Timer();
            timer.Elapsed += this.HandleTimerElapsed;
            timer.Interval = 50;
            return timer;
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            timerToThrottleOptionsUpdates.Stop();

            this.scenario.UpdateProjections();
            
            if (this.Accounts == null)
            {
                this.Accounts = new AccountsViewModel(scenario.Accounts);
            }
            else
            {
                this.Accounts.Bind(scenario.Accounts);
            }

            this.ScenarioOptions = new OptionsViewModel(scenario.Options);
            this.Projections = new ProjectionsViewModel(scenario.Projections);
            this.FilteredProjections = new ProjectionsViewModel(
                scenario.Projections
                .Where(projection => 
                    projection.YearNumber % 5 == 0 ||
                    projection.Year == this.Options.Spouse1.ProjectedYearOfDeath ||
                    projection.Year == this.Options.Spouse2.ProjectedYearOfDeath));
            this.ChartBars = new ChartViewModel(this);
            this.Boxes = new BoxesViewModel(scenario);
        }

        private void HandleOptionsChanged(object sender, EventArgs args)
        {
            // Update in 50ms, ignoring any further change notifications that happen
            // until the timer's elapsed event has been raised.
            timerToThrottleOptionsUpdates.Stop();
            timerToThrottleOptionsUpdates.Start();
        }
    }
}