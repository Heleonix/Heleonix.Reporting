document.addEventListener('DOMContentLoaded', function () {
    var assemblies = document.querySelectorAll('[data-role="item-assembly"]')
    var classes = document.querySelectorAll('[data-role="item-class"]')
    var testCases = document.querySelectorAll('[data-role="item-testcase"]')

    var assembliesSummary = document.querySelector('[data-role="summary-assemblies"]')
    var classesSummary = document.querySelector('[data-role="summary-classes"]')
    var testCasesSummary = document.querySelector('[data-role="summary-testcases"]')

    assemblies.forEach(function (e) { e.addEventListener('click', handleAssemblyLinkClick) })
    classes.forEach(function (e) { e.addEventListener('click', handleClassLinkClick) })

    testCases.forEach(function (e) {
        e.querySelector('[data-role="datetime-duration"]').innerText = formatTimeDuration(e.getAttribute('data-duration'))
    })

    updateSummary(assembliesSummary, testCases)
    updateSummary(classesSummary, testCases)
    updateSummary(testCasesSummary, testCases)

    var reportEndTime = document.querySelector('[data-role="report-datetime-end"]')
    reportEndTime.innerText = new Date(reportEndTime.getAttribute('data-end')).toLocaleString()

    function handleAssemblyLinkClick(e) {
        select(assemblies, e.currentTarget)
        select(classes, null)

        filter(classes, [e.currentTarget])

        var visibleClasses = []

        classes.forEach(function (e) { if (!e.attributes.hidden) visibleClasses.push(e) })

        filter(testCases, visibleClasses)

        updateSummary(classesSummary, testCases);
        updateSummary(testCasesSummary, testCases);
    }

    function handleClassLinkClick(e) {
        select(classes, e.currentTarget)

        filter(testCases, [e.currentTarget])

        updateSummary(testCasesSummary, testCases)
    }
}, false)

function select(list, current) {
    list.forEach(function (e) { e.removeAttribute('data-selected') })

    if (current) {
        current.setAttribute('data-selected', true)
    }
}

function filter(list, parents) {
    list.forEach(function (e) {
        if (!parents || parents.some(function (p) { return p.getAttribute('data-key') === e.getAttribute('data-parent-key') })) {
            e.removeAttribute('hidden')
        }
        else {
            e.setAttribute('hidden', true)
        }
    })
}

function updateSummary(summary, testCases) {
    var counts = { total: 0, passed: 0, failed: 0, error: 0, warning: 0, skipped: 0, other: 0, start: Date.now(), end: 0 }

    testCases.forEach(function (e) {
        if (!e.attributes.hidden) {
            counts.total += 1
            counts[e.attributes['data-outcome'].value] += 1

            var startDate = new Date(e.attributes['data-start'].value)
            var endDate = new Date(e.attributes['data-end'].value)

            if (startDate.valueOf() < counts.start) {
                counts.start = startDate.valueOf()
            }

            if (endDate.valueOf() > counts.end) {
                counts.end = endDate.valueOf()
            }
        }
    })

    summary.querySelector('[data-role="count-total"]').innerText = counts.total
    summary.querySelector('[data-role="count-passedrate"]').innerText =
        counts.total > 0 ? Math.round(counts.passed / counts.total * 100) : 0

    summary.querySelector('[data-role="count-passed"]').innerText = counts.total > 0 ? counts.passed : 0
    summary.querySelector('[data-role="count-warning"]').innerText = counts.total > 0 ? counts.warning : 0
    summary.querySelector('[data-role="count-failed"]').innerText = counts.total > 0 ? counts.failed : 0
    summary.querySelector('[data-role="count-error"]').innerText = counts.total > 0 ? counts.error : 0
    summary.querySelector('[data-role="count-skipped"]').innerText = counts.total > 0 ? counts.skipped : 0
    summary.querySelector('[data-role="count-other"]').innerText = counts.total > 0 ? counts.other : 0

    summary.querySelector('[data-role="datetime-start"]').innerText = counts.total > 0
        ? formatTime(new Date(counts.start)) : '00:00:00.000'
    summary.querySelector('[data-role="datetime-end"]').innerText = counts.total > 0
        ? formatTime(new Date(counts.end)) : '00:00:00.000'

    var txt = formatTimeDuration(counts.end - counts.start)

    summary.querySelector('[data-role="datetime-duration"]').innerText = counts.total > 0 ? txt : '00:00:00.000'

    var style = getComputedStyle(document.body)
    var pieTurnStop = 0
    var gradientString = counts.total > 0 ? 'conic-gradient('
        + style.getPropertyValue('--color-passed') + ' 0 ' + (pieTurnStop += counts.passed / counts.total) + 'turn,'
        + style.getPropertyValue('--color-warning') + ' 0 ' + (pieTurnStop += counts.warning / counts.total) + 'turn,'
        + style.getPropertyValue('--color-failed') + ' 0 ' + (pieTurnStop += counts.failed / counts.total) + 'turn,'
        + style.getPropertyValue('--color-error') + ' 0 ' + (pieTurnStop += counts.error / counts.total) + 'turn,'
        + style.getPropertyValue('--color-skipped') + ' 0 ' + (pieTurnStop += counts.skipped / counts.total) + 'turn,'
        + style.getPropertyValue('--color-other') + ' 0 ' + (pieTurnStop += counts.other / counts.total) + 'turn)'
        : 'conic-gradient(' + style.getPropertyValue('--color-undefined') + ' 0turn 1turn)'

    summary.querySelector('[data-role="chart-surface"]').style.backgroundImage = gradientString
}

function twoDigitsTime(t) {
    return t > 9 ? '' + t : '0' + t
}

function threeDigitsTime(t) {
    return t < 10 ? '00' + t : t < 100 ? '0' + t : '' + t
}

function formatTime(dt) {
    return twoDigitsTime(dt.getHours()) + ':'
        + twoDigitsTime(dt.getMinutes()) + ':'
        + twoDigitsTime(dt.getSeconds()) + '.'
        + threeDigitsTime(dt.getMilliseconds())
}

function formatTimeDuration(ms) {
    var txt = ''

    for (var msLeft = ms, timeScale = 60 * 60 * 1000; timeScale >= 1000; timeScale /= 60) {
        var time = Math.floor(msLeft / timeScale)
        txt += twoDigitsTime(time) + ':'
        msLeft = msLeft - time * timeScale
    }

    txt = txt.slice(0, -1) + '.' + threeDigitsTime(msLeft)

    return txt
}
