using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PropertiesOfExtractableResource
{
    public ReactiveProperty<int> DiscoveredDeposits = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> AvailableDeposits = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> ExtractedResources = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> AvailableResources = new ReactiveProperty<int>(0);
}
