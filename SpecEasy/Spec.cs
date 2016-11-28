using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace SpecEasy
{
    public partial class Spec
    {
        internal virtual void BeforeEachInit() { }

        protected virtual void BeforeEachExample() { }

        protected virtual void AfterEachExample() { }

        protected IContext Given(string description)
        {
            return Given(description, () => { });
        }

        protected IContext Given(string description, Action setup)
        {
            return Given(description, WrapAction(setup));
        }

        protected IContext Given(string description, Func<Task> setup)
        {
            return Given(description, setup, null);
        }

        private IContext Given(string description, Func<Task> setup, string conjunction)
        {
            Context context;
            var duplicateDescriptionContext = contexts.FirstOrDefault(c => c.Description == description);
            if (duplicateDescriptionContext != null)
            {
                context = new Context(ThrowDuplicateDescriptionException("context", description), description, conjunction);
                contexts.Remove(duplicateDescriptionContext);
            }
            else
            {
                context = new Context(setup, description, conjunction);
            }

            contexts.Add(context);
            return context;
        }

        protected IContext Given(Action setup)
        {
            return Given(WrapAction(setup));
        }

        protected IContext Given(Func<Task> setup)
        {
            var context = new Context(setup);
            contexts.Add(context);
            return context;
        }

        protected IContext And(string description, Action setup)
        {
            return And(description, WrapAction(setup));
        }

        protected IContext And(string description, Func<Task> setup)
        {
            return Given(description, setup, Context.AndConjunction);
        }

        protected IContext And(string description)
        {
            return And(description, () => { });
        }

        protected IContext But(string description, Action setup)
        {
            return But(description, WrapAction(setup));
        }

        protected IContext But(string description, Func<Task> setup)
        {
            return Given(description, setup, Context.ButConjunction);
        }

        protected IContext But(string description)
        {
            return But(description, () => { });
        }

        protected virtual IContext ForWhen(string description, Action action)
        {
            var context = new Context(async () => { }, () => When(description, action));
            contexts.Add(context);
            return context;
        }

        protected virtual void When(string description, Action action)
        {
            When(description, WrapAction(action));
        }

        protected virtual void When(string description, Func<Task> func)
        {
            when = new KeyValuePair<string, Func<Task>>(description, func);
        }

        protected IVerifyContext Then(string description, Action specification)
        {
            return Then(description, WrapAction(specification));
        }

        protected IVerifyContext Then(string description, Func<Task> specification)
        {
            if (thens.ContainsKey(description))
            {
                thens[description] = ThrowDuplicateDescriptionException("then", description);
            }
            else
            {
                thens[description] = specification;
            }

            return new VerifyContext(Then);
        }

        protected void Raise<T>(Action<T> eventSubscription, params object[] args) where T : class
        {
            var mock = Get<T>();
            mock.Raise(eventSubscription, args);
        }

        protected void AssertWasThrown<T>() where T : Exception
        {
            AssertWasThrown<T>(null);
        }

        protected void AssertWasThrown<T>(Action<T> expectation) where T : Exception
        {
            exceptionAsserted = true;
            var expectedException = thrownException as T;
            if (expectedException == null)
            {
                throw new Exception("Expected exception was not thrown");
            }

            if (expectation != null)
            {
                try
                {
                    expectation(expectedException);
                }
                catch (Exception exc)
                {
                    var message = string.Format(
                        "The expected exception type was thrown but the specified constraint failed. Constraint Exception: {0}{1}",
                        Environment.NewLine, exc.Message
                    );
                    throw new Exception(message, exc);
                }
            }

            thrownException = null;
        }

        protected void AssertWasCalled<T>(Action<T> action)
        {
            var mock = Get<T>();
            mock.AssertWasCalled(action);
        }

        protected void AssertWasCalled<T>(Action<T> action, Action<IMethodOptions<object>> methodOptions)
        {
            var mock = Get<T>();
            mock.AssertWasCalled(action, methodOptions);
        }

        protected void AssertWasNotCalled<T>(Action<T> action)
        {
            var mock = Get<T>();
            mock.AssertWasNotCalled(action);
        }

        protected void AssertWasNotCalled<T>(Action<T> action, Action<IMethodOptions<object>> methodOptions)
        {
            var mock = Get<T>();
            mock.AssertWasNotCalled(action, methodOptions);
        }

        private static Func<Task> ThrowDuplicateDescriptionException(string typeOfDuplicate, string description)
        {
            var stackTrace = Environment.StackTrace;
            return () => { throw new DuplicateDescriptionException(typeOfDuplicate, description, stackTrace); };
        }

        private static Func<Task> WrapAction(Action action)
        {
            return async () => action();
        }
    }
}