﻿using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class GameInstaller : MonoBehaviour, IInstaller
{
	public Camera mainCamera;
	public PaperPlane plane;

	public void RegisterBindings(DiContainer container)
	{
	    InstallerUtil.InstallUnityStandard(container);

	    container.Bind<IDependencyRoot>().AsSingle<GameRoot>();

	    container.Bind<Camera>().AsSingle(mainCamera);

		container.Bind<PaperPlane>().AsSingle(plane);

	    container.Bind<IEntryPoint>().AsSingle<GameController>();
	    container.Bind<ITickable>().AsSingle<GameController>();
	    container.Bind<GameController>().AsSingle();
	}

	
	// - The root of the object graph for our main run config
	public class GameRoot : DependencyRootStandard
	{
	}
}
