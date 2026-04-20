using System.Collections.Generic;
using UnityEngine;

namespace ModestTree.Zenject
{
    public class DependencyRootStandard : IDependencyRoot
    {
        // Usually we pass dependencies via contructor injection
        // but since we define a root so often (eg. unit tests)
        // just use [Inject] for the root classes

        #pragma warning disable 0414
        [Inject]
        IKernel _kernel = null;
        #pragma warning restore 0414

        [Inject]
        EntryPointInitializer _initializer = null;

        protected virtual void AssertDependenciesResolved()
        {
            // These injections intentionally force the bootstrapping services to resolve.
            _ = _kernel;
            _ = _initializer;
            Assert.That(_kernel != null);
            Assert.That(_initializer != null);
        }

        public virtual void Start()
        {
            AssertDependenciesResolved();
            Debug.Log("Initializing dependency root");
            _initializer.Initialize();
        }
    }

    public class DependencyRootStandard<TRoot> : DependencyRootStandard
        where TRoot : class
    {
        #pragma warning disable 0414
        [Inject]
        TRoot _root = null;
        #pragma warning restore 0414

        protected override void AssertDependenciesResolved()
        {
            base.AssertDependenciesResolved();
            _ = _root;
            Assert.That(_root != null);
        }
    }
}
