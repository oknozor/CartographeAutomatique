# Changelog
All notable changes to this project will be documented in this file. See [conventional commits](https://www.conventionalcommits.org/) for commit guidelines.

- - -
## [0.1.0](https://github.com/cocogitto/CartographeAutomatique/compare/204e77b5288be02408d7988fc8d17fc3fa91a5c1..0.1.0) - 2024-12-14
#### Continuous Integration
- add cocogitto release - ([21fe094](https://github.com/cocogitto/CartographeAutomatique/commit/21fe0942278fc5dedfa8ee199c4f8a153e591ec8)) - [@oknozor](https://github.com/oknozor)
- add github workflow ci.yml - ([c0540a4](https://github.com/cocogitto/CartographeAutomatique/commit/c0540a4c348efe2d0d37bdb3490a3769ac82a689)) - [@oknozor](https://github.com/oknozor)
#### Documentation
- more doc preparation - ([e7b00d2](https://github.com/cocogitto/CartographeAutomatique/commit/e7b00d287a3bc922bf3ba4a5f3b8b0f85e2da5b4)) - [@oknozor](https://github.com/oknozor)
- add doc skeleton - ([2180258](https://github.com/cocogitto/CartographeAutomatique/commit/2180258fe51dc03231bdb076d4d8ef96c2d328ad)) - [@oknozor](https://github.com/oknozor)
#### Features
- support lowerCase field match - ([14f6230](https://github.com/cocogitto/CartographeAutomatique/commit/14f623025ccbcd901faa2ca46c9fe1afe75a1c4c)) - [@oknozor](https://github.com/oknozor)
- add from mappings - ([46b8675](https://github.com/cocogitto/CartographeAutomatique/commit/46b867560787746086ff7439a70b2c617a495af8)) - [@oknozor](https://github.com/oknozor)
- support recursive list type implicit mapping - ([39216c0](https://github.com/cocogitto/CartographeAutomatique/commit/39216c0ab614594dd6d0321c93415efed163bcbc)) - [@oknozor](https://github.com/oknozor)
- add list to list conversion - ([49bdec2](https://github.com/cocogitto/CartographeAutomatique/commit/49bdec28a3d4ba81850485f6408033c86d53da44)) - [@oknozor](https://github.com/oknozor)
- implement implicit mapping for some primitive types - ([cb9018a](https://github.com/cocogitto/CartographeAutomatique/commit/cb9018a21cd95395c00af52f6f91e11a59ec6ef9)) - [@oknozor](https://github.com/oknozor)
- base skeleton for implicit mappings - ([38f35df](https://github.com/cocogitto/CartographeAutomatique/commit/38f35df3a56c13ebcb56cb84793346469b971099)) - [@oknozor](https://github.com/oknozor)
- add mapping via custom method - ([0719ba1](https://github.com/cocogitto/CartographeAutomatique/commit/0719ba162e4ff7d7d656bcad93087f60b592f0c8)) - [@oknozor](https://github.com/oknozor)
- add constructor class mappings for class - ([a900ccc](https://github.com/cocogitto/CartographeAutomatique/commit/a900ccc7f2209c51494f7967ddc034111fb436b1)) - [@oknozor](https://github.com/oknozor)
- drive constructor or setter instanciation via MappingStrategy - ([d7d2ffe](https://github.com/cocogitto/CartographeAutomatique/commit/d7d2ffed1cecaf4c8a939ba1900fade43f9c9833)) - [@oknozor](https://github.com/oknozor)
- add support for implicit target field mapping and multiple field mapping - ([f33d973](https://github.com/cocogitto/CartographeAutomatique/commit/f33d9737f68dd9d351b77f8202340a518826622b)) - [@oknozor](https://github.com/oknozor)
- support target field mapping in constructor parameters - ([b996f59](https://github.com/cocogitto/CartographeAutomatique/commit/b996f592db874658bfdcd2731dce60466dae2de4)) - [@oknozor](https://github.com/oknozor)
- support record mappings - ([33d794b](https://github.com/cocogitto/CartographeAutomatique/commit/33d794b35baf4ac19600f86d721686f4a2e3d0eb)) - [@oknozor](https://github.com/oknozor)
- target field mapping - ([9c28ea8](https://github.com/cocogitto/CartographeAutomatique/commit/9c28ea829e92d6b0abcd2ebf06568fea1345c123)) - [@oknozor](https://github.com/oknozor)
- add naive recursive mapping - ([0f0de81](https://github.com/cocogitto/CartographeAutomatique/commit/0f0de81892568bb44f6494f5833136480ec94efd)) - [@oknozor](https://github.com/oknozor)
- add non exhausive mapping and allow multiple mappings - ([fc4322e](https://github.com/cocogitto/CartographeAutomatique/commit/fc4322e44e03edee6e3b8e399b7d0290a65b64a0)) - [@oknozor](https://github.com/oknozor)
#### Miscellaneous Chores
- sanitize generated namespaces - ([2046cc6](https://github.com/cocogitto/CartographeAutomatique/commit/2046cc6ca236a9c3857b5edc47bbc41fa4e9909d)) - [@oknozor](https://github.com/oknozor)
- prepare for more ICollection type mappings - ([343dc7c](https://github.com/cocogitto/CartographeAutomatique/commit/343dc7c291a7067ff6c56afb30674e229b592077)) - [@oknozor](https://github.com/oknozor)
- rename target mapping attribute - ([8710f69](https://github.com/cocogitto/CartographeAutomatique/commit/8710f69a9789a3555587e205754d2669eb0d8445)) - [@oknozor](https://github.com/oknozor)
- format - ([2dafa69](https://github.com/cocogitto/CartographeAutomatique/commit/2dafa697abc1ad962141f12ed7cd1a48cc590d8d)) - [@oknozor](https://github.com/oknozor)
- remove unused solution - ([492994c](https://github.com/cocogitto/CartographeAutomatique/commit/492994cd2c5a3172e8bc2560b11f54f762710847)) - [@oknozor](https://github.com/oknozor)
- remove user specific stuff that breaks restore on other computers - ([da1ee92](https://github.com/cocogitto/CartographeAutomatique/commit/da1ee92eb90716651496fc1e804ccb4ef4d670bb)) - Lucas Declercq
#### Refactoring
- use attribute syntax generator context - ([441640f](https://github.com/cocogitto/CartographeAutomatique/commit/441640f635eb40c5d3b4c0ae24231d9bbce5eed8)) - [@oknozor](https://github.com/oknozor)
- extract method for list mappings - ([bdad3d9](https://github.com/cocogitto/CartographeAutomatique/commit/bdad3d96da148439d128acad322bdd4a9ee543fd)) - [@oknozor](https://github.com/oknozor)
- separate samples from tests - ([43a0e99](https://github.com/cocogitto/CartographeAutomatique/commit/43a0e994d07d56c70453b34b74c05e7bddca6c56)) - [@oknozor](https://github.com/oknozor)
- factorize test harness - ([13e3959](https://github.com/cocogitto/CartographeAutomatique/commit/13e39591152724abee33566ed330a1d5e75f3f9a)) - [@oknozor](https://github.com/oknozor)
- extract syntax helper - ([2754007](https://github.com/cocogitto/CartographeAutomatique/commit/2754007a2d6f172ffc0a2a2a2e2ea229f3173d25)) - [@oknozor](https://github.com/oknozor)
- use method extension instead of partial class for mappings - ([e1d2205](https://github.com/cocogitto/CartographeAutomatique/commit/e1d22055805f333c842e0dcd36c8087b36f0d934)) - [@oknozor](https://github.com/oknozor)
- extract classMapping - ([907cd41](https://github.com/cocogitto/CartographeAutomatique/commit/907cd4172249ac51f9557548da742204f6386b58)) - [@oknozor](https://github.com/oknozor)
#### Tests
- factorize generator tests - ([ad77de1](https://github.com/cocogitto/CartographeAutomatique/commit/ad77de114564c21391b57c6a21989eca09f5ee01)) - [@oknozor](https://github.com/oknozor)

- - -

Changelog generated by [cocogitto](https://github.com/cocogitto/cocogitto).